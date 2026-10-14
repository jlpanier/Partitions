using CommunityToolkit.Maui.Storage;
using FFImageLoading.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class ImportViewModel : BaseViewModel
    {
        public ICommand ClickFolder => new Command(OnFolder);
        
        /// <summary>
        /// Import des données
        /// </summary>
        public ICommand ClickTag => new Command<TagViewModel>(OnTag);

        /// <summary>
        /// Import des données
        /// </summary>
        public ICommand ClickImport => new Command(OnImport);

        /// <summary>
        /// Annuler 
        /// </summary>
        public ICommand ClickCancel => new Command(OnCancel);

        /// <summary>
        /// Ensembles des lignes
        /// </summary>
        public string Folder
        {
            get => _folder;
            set
            {
                if (_folder != value)
                {
                    _folder = value;
                    NotifyPropertyChanged(nameof(Folder));
                }
            }
        }
        public string _folder = "";


        /// <summary>
        /// Ensembles des lignes
        /// </summary>
        public ObservableCollection<TagViewModel> Tags
        {
            get => _tags;
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    NotifyPropertyChanged(nameof(Tags));
                }
            }
        }
        public ObservableCollection<TagViewModel> _tags = new ObservableCollection<TagViewModel>();


        public void Init()
        {
            Tags = new ObservableCollection<TagViewModel>(TagViewModel.All);
            foreach (var item in Tags)
            {
                item.IsSelected = false;
            }
            Folder = Business.Settings.Instance.ImportFolder;
        }

        /// <summary>
        /// Visualisation de la facture
        /// </summary>
        public async void OnImport()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Folder))
                {
                    throw new Exception("Veuillez sélectionner un dossier d'importation.");
                }
                if (Directory.Exists(Business.Appli.Instance.TmpPath) == false)
                {
                    throw new Exception("Le dossier d'importation n'existe pas.");
                }
                int nbr = 0;
                var directories = Directory.GetDirectories(Business.Appli.Instance.TmpPath, "*");
                // directories[0] = /data/user/0/com.jelosperone.partitions/files/Documents/tmp/duet
                foreach (var directory in directories)
                {
                    var subdirectories = Directory.GetDirectories(directory, "*");
                    if (subdirectories.Any())
                    {
                        foreach (var subdirectory in subdirectories)
                        {
                            var files = Directory.GetFiles(subdirectory, "*");
                            if (files.Any())
                            {
                                await Business.Partition.Import(files, Tags.Where(t => t.IsSelected).Select(_ => _.Item));
                                nbr++;
                            }
                        }
                    }
                }
                Directory.Delete(Business.Appli.Instance.TmpPath, recursive: true);
                Directory.CreateDirectory(Business.Appli.Instance.TmpPath);

                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync($"{nbr} partitions importées");
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Annuler 
        /// </summary>
        public async void OnCancel()
        {
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }

        /// <summary>
        /// Chargement des données 
        /// </summary>
        public async void OnTag(TagViewModel tag)
        {
            try
            {
                tag.Select();
                NotifyPropertyChanged(nameof(Tags));
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Chargement des données 
        /// </summary>
        public async void OnFolder()
        {
            try
            {
                try
                {
                    var result = await FilePicker.Default.PickAsync(new PickOptions
                    {
                        PickerTitle = "Choisir une facture au format PDF",
                    });

                    if (result != null)
                    {
                        var destFileName = Path.Combine(Business.Appli.Instance.TmpPath, result.FileName);
                        System.IO.Compression.ZipFile.ExtractToDirectory(result.FullPath, Business.Appli.Instance.TmpPath, overwriteFiles: true);
                        Folder = result.FileName;
                    }
                }
                catch (Exception ex)
                {
                    await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
                }

                //var result = await FolderPicker.Default.PickAsync();

                //if (result.IsSuccessful)
                //{
                //    var folderPath = result.Folder.Path;
                //    var folderName = result.Folder.Name;
                //    var files = Directory.GetFiles(folderPath, "*");

                //    Folder = result.Folder.Path;
                //    //await Toast.Make($"Dossier sélectionné : {folderName}\n{folderPath}").Show(cancellationToken);
                //}
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }
    }
}
