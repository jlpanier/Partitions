using Business;
using FFImageLoading.Helpers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion de la page principale
    /// </summary>
    public partial class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Exporter les partitions dans un fichier
        /// </summary>
        public ICommand ClickZip => new Command(OnZip);

        /// <summary>
        /// Selection d'un tag
        /// </summary>
        public ICommand ClickTag => new Command<TagViewModel>(OnSelectedTag);

        /// <summary>
        /// Suppression de toute sélection des tags
        /// </summary>
        public ICommand ClickClearTags => new Command(OnClickClearTags);

        /// <summary>
        /// Affichage de la page des tags
        /// </summary>
        public ICommand ClickTags => new Command(OnTags);

        /// <summary>
        /// Edition de la partition
        /// </summary>
        public ICommand ClickEdit => new Command<PartitionViewModel>(OnEdit);

        /// <summary>
        /// Import de nouvelles partitions via un zip file
        /// </summary>
        public ICommand ClickImport => new Command(OnImport);

        /// <summary>
        /// Gestion de la configuration
        /// </summary>
        public ICommand ClickSettings => new Command(OnSettings);

        /// <summary>
        /// Clic sur le bouton du menu
        /// </summary>
        public ICommand ClicMenu => new Command(OnMenu);

        /// <summary>
        /// VRAI, si au moins un tag sélectionné
        /// </summary>
        public bool HasSelectedTags
        {
            get => _hasSelectedTags;
            set
            {
                if (_hasSelectedTags != value)
                {
                    _hasSelectedTags = value;
                    NotifyPropertyChanged(nameof(HasSelectedTags));
                }
            }
        }
        private bool _hasSelectedTags = true;

        /// <summary>
        /// VRAI, si le popup menu doit être visible
        /// </summary>
        public bool MenuVisible
        {
            get => _menuVisible;
            set
            {
                _menuVisible = value;
                NotifyPropertyChanged(nameof(MenuVisible));
            }
        }
        private bool _menuVisible;

        /// <summary>
        /// Zone de recherche du texte de recherche
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    NotifyPropertyChanged(nameof(SearchText));
                    Load(_searchText);
                }
            }
        }
        private string _searchText = string.Empty;

        /// <summary>
        /// Affichage le nombre de paritions sélectionnées et le nombre de partitions totales
        /// </summary>
        public string SearchStatusText
        {
            get => _searchStatusText;
            set
            {
                if (_searchStatusText != value)
                {
                    _searchStatusText = value;
                    NotifyPropertyChanged(nameof(SearchStatusText));
                }
            }
        }
        private string _searchStatusText = string.Empty;

        /// <summary>
        /// Ensembles des selectedtags possibles
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
        /// Ensembles des partitions sélectionnées 
        /// </summary>
        public ObservableCollection<PartitionViewModel> Items
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
        public ObservableCollection<PartitionViewModel> _items = new ObservableCollection<PartitionViewModel>();

        public MainViewModel()
        {
        }

        /// <summary>
        /// Edition du paramétrage
        /// </summary>
        private async void OnSettings()
        {
            MenuVisible = false;
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        /// <summary>
        /// Affichage du menu
        /// </summary>
        private async void OnMenu()
        {
            MenuVisible = !MenuVisible;
        }

        /// <summary>
        /// Initialisation 
        /// </summary>
        public void Init()
        {
            if (!Tags.Any())
            {
                // Dans le cas du rafraichissement les selectedtags sont déjà initialisés
                Tags = new ObservableCollection<TagViewModel>(TagViewModel.All);
            }
            Load(SearchText);
        }

        /// <summary>
        /// Chargement des selectedPartitions
        /// </summary>
        public void Load(string search ="")
        {

            // tags sélectionnés
            var selectedtags = Tags.Where(_ => _.IsSelected).Select(_ => _.Item).ToList();
            List<Partition> tagSelectionPartitions;
            if (selectedtags.Count() == Tags.Count())
            {
                // Tous les tags => pas de traitement, toutes les selectedPartitions sont sélectionnées
                tagSelectionPartitions = Partition.All;
            }
            else if (selectedtags.Any()) // nous avons au moins un tag sélectionné
            {
                tagSelectionPartitions = new List<Partition>();
                foreach (var item in Partition.All)
                {
                    if (item.Tags.Any() && item.Contains(selectedtags))
                    {
                        tagSelectionPartitions.Add(item);
                    }
                }
            }
            else
            {
                // aucun tag sélectionné, la liste est vide
                tagSelectionPartitions = new List<Partition>();
            }

            // sélection des selectedPartitions à afficher
            var selectedPartitions = new List<PartitionViewModel>();
            foreach (var partition in tagSelectionPartitions)
            {
                if (string.IsNullOrWhiteSpace(search))
                {
                    selectedPartitions.Add(PartitionViewModel.From(partition));
                }
                else if (partition.Name.ToLower().Contains(_searchText.ToLower()))
                {
                    selectedPartitions.Add(PartitionViewModel.From(partition));
                }
            }
            Items = new ObservableCollection<PartitionViewModel>(selectedPartitions);
            SearchStatusText = $"{Items.Count}/{Partition.All.Count}";
        }

        /// <summary>
        /// Import de nouvelles partitions via un zip file
        /// </summary>
        private async void OnImport()
        {
            try
            {
                MenuVisible = !MenuVisible;
                await Shell.Current.GoToAsync($"{nameof(ImportPage)}", new Dictionary<string, object>
                {
                });
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Edition de la partition
        /// </summary>
        private async void OnEdit(PartitionViewModel item)
        {
            try
            {
                await Shell.Current.GoToAsync($"{nameof(EditPartitionPage)}", new Dictionary<string, object>
                {
                    ["PartitionId"] = item.Id,
                });
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Suppression de toutes les sélections des tags
        /// </summary>
        private async void OnClickClearTags()
        {
            try
            {
                foreach (var tag in Tags.Where(i => i.IsSelected))
                {
                    tag.Unselect();
                }   
                HasSelectedTags = false;
                Load(SearchText);
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Affichage de la page des tags
        /// </summary>
        private async void OnTags()
        {
            try
            {
                MenuVisible = !MenuVisible;
                await Shell.Current.GoToAsync($"{nameof(TagsPage)}", new Dictionary<string, object>
                {
                });
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Selection d'un tag
        /// </summary>
        private async void OnSelectedTag(TagViewModel tag)
        {
            try
            {
                tag.Toggle();
                HasSelectedTags = Tags.Any(i => i.IsSelected);
                Load(SearchText);
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Externaliser les partitions dans un fichier zip
        /// </summary>
        private async void OnZip()
        {
            try
            {
                MenuVisible = !MenuVisible;

                var files = Directory.GetFiles(Appli.Instance.FilePath, "Partition*.zip");
                foreach(var file in files)
                {
                    File.Delete(file);
                }
#if ANDROID
                var filename = $"Partition_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.zip";
                string zipFile = Path.Combine(DownloadFolderService.GetDownloadFolder(), Path.GetFileName(filename));

                if (File.Exists(zipFile))
                {
                    File.Delete(zipFile);
                }

                ZipFile.CreateFromDirectory(Appli.Instance.FilePath, zipFile);
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync($"Le fichier {filename} a été créé avec succès.");

#endif

            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }
    }
}
