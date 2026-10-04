using Business;
using FFImageLoading.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace Main.ViewModels
{
    /// <summary>
    /// Gestion de la page principale
    /// </summary>
    public partial class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Import des données
        /// </summary>
        public ICommand ClickEdit => new Command<PartitionViewModel>(OnEdit);

        /// <summary>
        /// Import des données
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
        /// Nom de l'objet 
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
        /// Nom de l'objet 
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
        /// Ensembles des lignes
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
        /// Chargement de toutes 
        /// </summary>
        public void Load(string search ="")
        {
            var partitions = new List<PartitionViewModel>();
            foreach (var partition in Partition.All)
            {
                if (string.IsNullOrWhiteSpace(search))
                {
                    partitions.Add(PartitionViewModel.From(partition));
                }
                else if (partition.Name.ToLower().Contains(_searchText.ToLower()))
                {
                    partitions.Add(PartitionViewModel.From(partition));
                }
            }
            Items = new ObservableCollection<PartitionViewModel>(partitions);
            SearchStatusText = $"{Items.Count}/{Partition.All.Count}";

        }

        /// <summary>
        /// Chargement des données 
        /// </summary>
        public async void OnImport()
        {
            try
            {
                MenuVisible = !MenuVisible;

                var repository = "C:\\Users\\jean-\\Documents\\Pick";
                var directories = Directory.GetDirectories(repository, "*");
                foreach(var directory in directories)
                {
                    var name = Path.GetFileName(directory);
                    var files = Directory.GetFiles(directory, "*");
                    if (files.Any())
                    {
                        var piece = Partition.Import(files);
                    }
                }
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Chargement des données 
        /// </summary>
        public async void OnEdit(PartitionViewModel item)
        {
            try
            {
                await Shell.Current.GoToAsync($"{nameof(EditPartitionPage)}", new Dictionary<string, object>
                {
                    ["SearchText"] = item.Id,
                });
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }
    }
}
