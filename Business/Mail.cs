using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des mails
    /// </summary>
    public class Mail
    {
        /// <summary>
        /// Liste des mails
        /// </summary>
        public static List<Mail> All
        {
            get
            {
                if (_all==null)
                {
                    _all = new List<Mail>();
                    var items = DatabaseAccess.Instance.GetMails();
                    foreach (var item in items)
                    {
                        _all.Add(new Mail(item));
                    }
                }
                return _all;
            }
        }
        private static List<Mail>? _all = null;

        /// <summary>
        /// Création d'une entité
        /// </summary>
        public static Mail Create(string name, string mail)
        {
            var item = new MailEntity
            {
                Name = name,
                Email = mail,
                DateMaj = DateTime.Now,
            };
            DatabaseAccess.Instance.Add(item);

            var result = new Mail(item);
            All.Add(result);
            return result;
        }

        /// <summary>
        /// Conversion de l'entité en configuration
        /// </summary>
        public static Mail From(MailEntity item) => new Mail(item);

        /// <summary>
        /// Entity lié à cet email
        /// </summary>
        public readonly MailEntity Item;

        private Mail(MailEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Identifiant de l'élément
        /// </summary>
        public int Id => Item.Id;

        /// <summary>
        /// Nom
        /// </summary>
        public string Name => Item.Name;

        /// <summary>
        /// Email
        /// </summary>
        public string Email => Item.Email;

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public int Update(string name, string email)
        {
            Item.DateMaj = DateTime.Now;
            Item.Name = name;
            Item.Email = email;
            return DatabaseAccess.Instance.Update(Item);
        }

        /// <summary>
        /// Supression
        /// </summary>
        public void Delete()
        {
            DatabaseAccess.Instance.Remove(Item);
        }
    }
}
