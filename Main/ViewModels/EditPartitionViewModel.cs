using Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class EditPartitionViewModel: BaseViewModel
    {        /// <summary>
             /// Enregistrer 
             /// </summary>
        public ICommand ClickSave => new Command(OnSave);

        /// <summary>
        /// Annuler 
        /// </summary>
        public ICommand ClickCancel => new Command(OnCancel);

        /// <summary>
        /// Annuler 
        /// </summary>
        public ICommand ClickDelete => new Command(OnDelete);

        /// <summary>
        /// Référence de la configuration
        /// </summary>
        public string Label
        {
            get => _label;
            set
            {
                if (_label != value)
                {
                    _label = value;
                    NotifyPropertyChanged(nameof(Label));
                }
            }
        }
        private string _label = "";

        /// <summary>
        /// Valeur de la configuration
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
        /// Description de la configuration
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
        /// Liste de la configuration
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

        private Partition? _partition = null;

        public EditPartitionViewModel()
        {
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int key)
        {
            _partition = Partition.All.FirstOrDefault(_ => _.Id == key);
            if (_partition != null)
            {
                Label = _partition.Label;
                Compositor = _partition.Compositor;
                Arranger = _partition.Arranger;
                Items = new ObservableCollection<Source>(_partition.Sources);
                //Duration = _partition.Duration;
            }
        }

        /// <summary>
        /// Sauvegarde de la balance mensuelle
        /// </summary>
        private async void OnSave()
        {
            if (_partition != null)
            {
                //_partition.Save(Label, Compositor, Arranger);
            }
            else
            {
                //Settings.Instance.Add(Label, Compositor, Arranger);
            }
            //await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }

        /// <summary>
        /// Annuler 
        /// </summary>
        public async void OnCancel()
        {
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }

        /// <summary>
        /// Supopression de la configuration 
        /// </summary>
        public async void OnDelete()
        {
            //if (_partition != null)
            //{
            //    _partition.Delete();
            //}
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }
    }
}
