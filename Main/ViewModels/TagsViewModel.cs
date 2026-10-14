using Business;
using CommunityToolkit.Maui.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class TagsViewModel : BaseViewModel
    {
        /// <summary>
        /// Ajout d'une facture
        /// </summary>
        public ICommand ClickAdd => new Command(OnAdd);

        /// <summary>
        /// Ajout d'une facture
        /// </summary>
        public ICommand ClickEdit=> new Command<Tag>(OnEdit);

        public void Init()
        {
            Items = new ObservableCollection<Tag>(Tag.All);
        }

        /// <summary>
        /// Ajout d'une facture
        /// </summary>
        public async void OnAdd()
        {
            var popup = new TagPopup(string.Empty);
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            if (result != null && !result.WasDismissedByTappingOutsideOfPopup && popup.ShouldRename)
            {
                Tag.Create(popup.NewName);
                Items = new ObservableCollection<Tag>(Tag.All);
            }

        }

        /// <summary>
        /// Ajout d'une facture
        /// </summary>
        public async void OnEdit(Tag item)
        {
            var popup = new TagPopup(item.Label);
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            if (result != null && !result.WasDismissedByTappingOutsideOfPopup && popup.ShouldRename)
            {
                item.Update(popup.NewName);
                Items = new ObservableCollection<Tag>(Tag.All);
            }
            else if (result != null && !result.WasDismissedByTappingOutsideOfPopup && popup.ShouldDelete)
            {
                item.Delete();
                Items = new ObservableCollection<Tag>(Tag.All);
            }
        }

        /// <summary>
        /// Ensembles des lignes de maintenances
        /// </summary>
        public ObservableCollection<Business.Tag> Items
        {
            get => _items;
            set
            {
                _items = value;
                NotifyPropertyChanged(nameof(Items));
            }
        }
        private ObservableCollection<Business.Tag> _items = new ObservableCollection<Business.Tag>();

        public void Load()
        {
            Items = new ObservableCollection<Business.Tag>(Business.Tag.All.OrderByDescending(_ => _.Label));
        }
    }

}
