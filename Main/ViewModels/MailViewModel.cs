using System.Windows.Input;

namespace Main.ViewModels
{
    public class MailViewModel: BaseViewModel
    {
        /// <summary>
        /// Supprimer cet email
        /// </summary>
        public ICommand ClickDelete => new Command(OnDelete);

        public static List<MailViewModel> All => From(Business.Mail.All);

        public static List<MailViewModel> From(List<Business.Mail> items) => items.Select(_=> From(_)).ToList();

        public static MailViewModel From(Business.Mail item) => new MailViewModel(item);

        /// <summary>
        /// Identifiant de l'élément
        /// </summary>
        public int Id => Item.Id;

        /// <summary>
        /// Nom associé à l'email
        /// </summary>
        public string Name => Item.Name;

        /// <summary>
        /// Email
        /// </summary>
        public string EMail => Item.Email;

        /// <summary>
        /// VRAI, si sélectionné pour l'envoi d'email
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NotifyPropertyChanged(nameof(IsSelected));
                }
            }
        }
        private bool _isSelected = true;

        public readonly Business.Mail Item;
        private MailViewModel(Business.Mail item)
        {
            Item = item;
        }

        private void OnDelete()
        {
            var mail = Business.Mail.All.FirstOrDefault(_ => _.Id == Id);
            if (mail != null)
            {
                Business.Mail.All.Remove(mail);
                mail.Delete();
            }
        }
    }
}
