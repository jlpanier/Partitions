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
        /// Gestion de la configuration
        /// </summary>
        public ICommand ClicSettings => new Command(OnSettings);

        /// <summary>
        /// Evenement de la mise à jour du paramétrages
        /// </summary>
        public ICommand ClickSettingsCommand => new Command(OnSettings);

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
        private string _name = string.Empty;

        /// <summary>
        /// Montant total dépensé pour le bateau
        /// </summary>
        public string TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    NotifyPropertyChanged(nameof(TotalAmount));
                }
            }
        }
        private string _totalAmount = string.Empty;

        /// <summary>
        /// Montant annuel dépensé pour le bateau
        /// </summary>
        public string AnnualAmount
        {
            get => _annualAmount;
            set
            {
                if (_annualAmount != value)
                {
                    _annualAmount = value;
                    NotifyPropertyChanged(nameof(AnnualAmount));
                }
            }
        }
        private string _annualAmount = string.Empty;

        /// <summary>
        /// Ensembles des lignes
        /// </summary>
        public ObservableCollection<Business.ILine> Lines
        {
            get => _lines;
            set
            {
                if (_lines != value)
                {
                    _lines = value;
                    NotifyPropertyChanged(nameof(Lines));
                }
            }
        }
        public ObservableCollection<Business.ILine> _lines = new ObservableCollection<Business.ILine>();

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
        /// Chargement de toutes les lignes de 
        /// </summary>
        public void Load()
        {
            
        }
    }
}
