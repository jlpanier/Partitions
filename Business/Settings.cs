using Repository.Dbo;
using System.Diagnostics;

namespace Business
{
    /// <summary>
    /// Gestion de la configuration
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }
        private static Settings? _instance;

        private Settings()
        {
        }

        /// <summary>
        /// Chargement de la configuration
        /// </summary>
        public List<Setting> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<Setting>();
                    DatabaseAccess.Instance.GetSettings().ToList().ForEach(_ => _all.Add(Setting.From(_)));
                }
                return _all;
            }
        }
        private List<Setting>? _all;

        /// <summary>
        /// Création d'une nouvelle entité
        /// </summary>
        public Setting Add(string key, string val, string descr)
        {
            var item = All.FirstOrDefault(_=>_.Key==key);
            if (item == null) 
            {
                item = Setting.Create(key, val, descr);
                All.Add(item);
            }
            else
            {
                item.Save(key, val, descr);
            }
            return item;
        }

        /// <summary>
        /// Obtenir la configuration d'une string
        /// </summary>
        private string GetString(string key, string defaultvalue)
        {
            var setting = All.FirstOrDefault(_=>_.Key == key);
            return setting == null ? defaultvalue : setting.Val;
        }

        /// <summary>
        /// Obtenir la configuration d'un entier
        /// </summary>
        private int GetInt(string key, int defaultvalue)
        {
            int result = defaultvalue;
            var setting = All.FirstOrDefault(_ => _.Key == key);
            if (setting != null)
            {
                if (int.TryParse(setting.Val, out int val))
                {
                    result = val;
                }
            }
            return result;
        }

        /// <summary>
        /// Obtenir la configuration d'un double
        /// </summary>
        private double GetDouble(string key, double defaultvalue)
        {
            double result = defaultvalue;
            var setting = All.FirstOrDefault(_ => _.Key == key);
            if (setting != null)
            {
                if (double.TryParse(setting.Val, out double val))
                {
                    result = val;
                }
            }
            return result;
        }

        /// <summary>
        /// Nom par defaut du fournisseur pour les factures
        /// </summary>
        public string InvoiceSupplierDefault => GetString("invoice.supplier.default", "Hunyvers Nautic - AD Nautic");

        /// <summary>
        /// Nom par defaut du fournisseur pour les factures
        /// </summary>
        public string WorkNameDefault => GetString("work.supplier.default", "Travaux");

        /// <summary>
        /// Largeur des images
        /// </summary>
        public int ImageWidthRequest => GetInt("image.width.default", 72);

        /// <summary>
        /// Hauteur des images
        /// </summary>
        public int ImageHeightRequest => GetInt("image.height.default", 72);

        /// <summary>
        /// Nom par defaut du répertoire de chargement
        /// </summary>
        public string ImportFolder => GetString("import.folder", "");

        /// <summary>
        /// Nom par defaut de l'application (popup)
        /// </summary>
        public string Name => GetString("application.name", "Partitions");

        /// <summary>
        /// Nom de l'envoi du mail
        /// </summary>
        public string noreply => GetString("application.mailbox.answer", "noreply@partition.com");

        /// <summary>
        /// Source mail
        /// </summary>
        public string Email => GetString("application.mailbox.mail", "jluc.panier@gmail.com");

        /// <summary>
        /// Source mail
        /// </summary>
        public string Pawd => GetString("application.mailbox.pawd", "xjeg xmyy icmy rjyk");
    }
}
