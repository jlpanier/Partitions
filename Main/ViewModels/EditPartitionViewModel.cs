using Business;
using FFImageLoading.Helpers;
using Repository.Dbo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;

namespace Main.ViewModels
{
    public class EditPartitionViewModel: BaseViewModel
    {
        /// <summary>
        /// Enregistrer 
        /// </summary>
        public ICommand ClickEdit => new Command<Source>(OnEdit);

        /// <summary>
        /// Enregistrer 
        /// </summary>
        public ICommand ClickView => new Command<Source>(OnView);

        /// <summary>
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

        public ObservableCollection<string> MinutesList => new ObservableCollection<string>(new List<string>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" });

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

        public ObservableCollection<string> SecondsList => new ObservableCollection<string>(new List<string>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" });
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
        /// Référence de la configuration
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
        /// Description de la configuration
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

        private Business.Partition? _partition = null;

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
                _partition = Business.Partition.All.FirstOrDefault(_ => _.Id == key);
                if (_partition != null)
                {
                    Name = _partition.Name;
                    Compositor = _partition.Compositor;
                    Arranger = _partition.Arranger;
                    Items = new ObservableCollection<Source>(_partition.Sources);

                    var ts = TimeSpan.FromSeconds(_partition.Duration);
                    SelectedMinutes = ts.Minutes.ToString("00");
                    SelectedSeconds = ts.Seconds.ToString("00");
                }
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Sauvegarde de la balance mensuelle
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
                        _partition.Save(Name, Compositor, Arranger, new TimeSpan(0, minutes, secondes));
                    }
                }
                await Shell.Current.GoToAsync(".."); // Retour à la page précédente
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
        /// Suppression de la configuration 
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
        /// Visualisation de la facture
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
                    await ServiceHelper.GetService<IAudioService>()!.PlayAsync(source.FilePath);
                    break;
            }
        }

        /// <summary>
        /// Visualisation de la facture
        /// </summary>
        public async void OnEdit(Source source)
        {
            System.Diagnostics.Debug.Assert(_partition != null, "_partition cannot be null");
            var popup = new RenamePopup(source.FileName);
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            if (result !=null && !result.WasDismissedByTappingOutsideOfPopup && popup.IsSuccess)
            {
                source.Rename(popup.NewName);
                Items = new ObservableCollection<Source>(_partition.Sources);
            }
        }
    }
}
