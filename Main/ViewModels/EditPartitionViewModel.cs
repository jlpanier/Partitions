using Business;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Markup;
using FFImageLoading.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion visualisation / édition d'une partition
    /// </summary>
    public class EditPartitionViewModel: BaseViewModel
    {
        /// <summary>
        /// Modification de l'état d'un tag (sélectionné / non sélectionné)
        /// </summary>
        public ICommand ClickMail => new Command(OnMail);

        /// <summary>
        /// Modification de l'état d'un tag (sélectionné / non sélectionné)
        /// </summary>
        public ICommand ClickTag => new Command<TagViewModel>(OnTag);

        /// <summary>
        /// Ouverture d'une popup pour renommer / archiver /supprimer le fichier
        /// </summary>
        public ICommand ClickEdit => new Command<Source>(OnEdit);

        /// <summary>
        /// Visualisation du fichier PDF ou MP3
        /// </summary>
        public ICommand ClickView => new Command<Source>(OnView);

        /// <summary>
        /// Sauvegarde des modifications
        /// </summary>
        public ICommand ClickSave => new Command(OnSave);

        /// <summary>
        /// Annuler 
        /// </summary>
        public ICommand ClickCancel => new Command(OnCancel);

        /// <summary>
        /// Supprimer cette partitions
        /// </summary>
        public ICommand ClickDelete => new Command(OnDelete);

        /// <summary>
        /// Ajuster le nombre de minutes de la partition
        /// </summary>
        public ObservableCollection<string> MinutesList => new ObservableCollection<string>(new List<string>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" });

        /// <summary>
        /// Sélection de la durée des minutes
        /// </summary>
        public string SelectedMinutes
        {
            get => _selectedMinutes;
            set
            {
                if (_selectedMinutes != value)
                {
                    _selectedMinutes = value;
                    NotifyPropertyChanged(nameof(SelectedMinutes));
                }
            }
        }
        private string _selectedMinutes = "00";

        /// <summary>
        /// Ajuster le nombre de secondes de la partition
        /// </summary>
        public ObservableCollection<string> SecondsList => new ObservableCollection<string>(new List<string>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" });

        /// <summary>
        /// Sélection de la durée des secondes
        /// </summary>
        public string SelectedSeconds
        {
            get => _selectedSeconds;
            set
            {
                if (_selectedSeconds != value)
                {
                    _selectedSeconds = value;
                    NotifyPropertyChanged(nameof(SelectedSeconds));
                }
            }
        }
        private string _selectedSeconds = "00";

        /// <summary>
        /// Ensemble des tags disponibles pour la partition
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


        /// <summary>
        /// Nom de la partition
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        private string _name = "";

        /// <summary>
        /// Compositeur de la partition
        /// </summary>
        public string Compositor
        {
            get => _compositor;
            set
            {
                if (_compositor != value)
                {
                    _compositor = value;
                    NotifyPropertyChanged(nameof(Compositor));
                }
            }
        }
        private string _compositor = "";

        /// <summary>
        /// Arrangeur de la partition
        /// </summary>
        public string Arranger
        {
            get => _arranger;
            set
            {
                if (_arranger != value)
                {
                    _arranger = value;
                    NotifyPropertyChanged(nameof(Arranger));
                }
            }
        }
        private string _arranger = "";


        /// <summary>
        /// Durée de la partition
        /// </summary>
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    NotifyPropertyChanged(nameof(Duration));
                }
            }
        }
        private TimeSpan _duration = TimeSpan.FromSeconds(0);

        /// <summary>
        /// Liste des fichiers constituant la partition (PDF, MuseScore, SND)
        /// </summary>
        public ObservableCollection<Source> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    NotifyPropertyChanged(nameof(Items));
                }
            }
        }
        public ObservableCollection<Source> _items = new ObservableCollection<Source>();

        /// <summary>
        /// Partition en cours d'édition
        /// </summary>
        private Business.Partition? _partition = null;

        /// <summary>
        /// Montrer ou non les fichiers archivés dans la liste des fichiers de la partition
        /// </summary>
        public bool ShowArchivedFiles
        {
            get => _showArchivedFiles;
            set
            {
                if (_showArchivedFiles != value)
                {
                    _showArchivedFiles = value;
                    NotifyPropertyChanged(nameof(ShowArchivedFiles));
                    Load();
                }
            }
        }
        private bool _showArchivedFiles = false;

        public EditPartitionViewModel()
        {
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public async void Init(int key)
        {
            try
            {
                Tags = new ObservableCollection<TagViewModel>(TagViewModel.All);
                _partition = Business.Partition.All.FirstOrDefault(_ => _.Id == key);
                if (_partition != null)
                {
                    Name = _partition.Name;
                    Compositor = _partition.Compositor;
                    Arranger = _partition.Arranger;
                    

                    var ts = TimeSpan.FromSeconds(_partition.Duration);
                    SelectedMinutes = ts.Minutes.ToString("00");
                    SelectedSeconds = ts.Seconds.ToString("00");

                    foreach(var tag in Tags)
                    {
                        tag.IsSelected = _partition.Tags.Any(_=>_.Id == tag.Id);
                    }

                    Load();
                }
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        private void Load()
        {
            if (_partition == null)
            {
                Items = new ObservableCollection<Source>();
            }
            else if (ShowArchivedFiles)
            {
                Items = new ObservableCollection<Source>(_partition.Sources);
            }
            else
            {
                Items = new ObservableCollection<Source>(_partition.Sources.Where(_=>!_.IsArchived));
            }
        }

        /// <summary>
        /// Sauvegarde des modifications
        /// </summary>
        private async void OnSave()
        {
            try
            {
                ServiceHelper.GetService<IAudioService>()!.Stop();
                if (int.TryParse(SelectedMinutes, out int minutes) && int.TryParse(SelectedSeconds, out int secondes))
                {
                    if (_partition != null)
                    {
                        _partition.Save(Name, Compositor, Arranger, new TimeSpan(0, minutes, secondes), Tags.Where(_=>_.IsSelected).Select(_=>_.Item));
                    }
                }
                await Shell.Current.GoToAsync("..", new Dictionary<string, object>
                {
                }); 
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
            ServiceHelper.GetService<IAudioService>()!.Stop();
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }

        /// <summary>
        /// Suppression de la partition
        /// </summary>
        public async void OnDelete()
        {
            ServiceHelper.GetService<IAudioService>()!.Stop();
            if (_partition == null)
            {
                await Shell.Current.GoToAsync(".."); // Retour à la page précédente
                return;
            }
            var result = await ServiceHelper.GetService<IAlertService>()!.ShowConfirmationAsync("Confirmation", $"Supprimer la partition \"{_partition.Name}\"?", "Oui", "Non");
            if (result)
            {
                _partition.Delete();
                await Shell.Current.GoToAsync(".."); // Retour à la page précédente
                return;
            }
        }

        /// <summary>
        /// Visualisation de la partition
        /// </summary>
        public async void OnView(Source source)
        {
            switch (source.Nature)
            {
                case Source.NatureType.Unknown:
                    break;
                case Source.NatureType.MuseScore:
                    break;
                case Source.NatureType.PDF:
                    PdfOpener.OpenPdf(source.FilePath);
                    break;
                case Source.NatureType.SND:
                    ServiceHelper.GetService<IAudioService>()!.Stop();
                    if (_partition!=null && _partition.Duration==0)
                    {
                        var file = TagLib.File.Create(source.FilePath);
                        _partition.SetDuration(file.Properties.Duration);
                    }
                    await ServiceHelper.GetService<IAudioService>()!.PlayAsync(source.FilePath);
                    break;
                case Source.NatureType.VIDEO:
                    await ServiceHelper.GetService<IAudioService>()!.PlayAsync(source.FilePath);
                    break;
            }
        }

        /// <summary>
        /// Ouverture d'une popup pour renommer / archiver /supprimer le fichier
        /// </summary>
        private async void OnEdit(Source source)
        {
            System.Diagnostics.Debug.Assert(_partition != null, "_partition cannot be null");
            var popup = new FileActionPopup(source.FileName);
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            if (result !=null && !result.WasDismissedByTappingOutsideOfPopup)
            {
                if (popup.IsDelete)
                {
                    source.Delete();
                }
                else if (popup.IsArchive)
                {
                    source.Archive();
                }
                else if (popup.IsRename)
                {
                    source.Rename(popup.FileName);
                }
                Load();
            }
        }

        /// <summary>
        /// Modification de l'état d'un tag (sélectionné / non sélectionné)
        /// </summary>
        private async void OnTag(TagViewModel tag)
        {
            try
            {
                tag.Toggle();
                NotifyPropertyChanged(nameof(Tags));
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Envoi de la partition à des destinataires
        /// </summary>
        private async void OnMail()
        {
            try
            {
                System.Diagnostics.Debug.Assert(_partition != null, "_partition cannot be null");
                var popup = new SendMailPopup(_partition);
                var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }
    }
}
