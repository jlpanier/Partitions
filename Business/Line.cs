using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des lignes 
    /// </summary>
    public class Line 
    {
        /// <summary>
        /// Référence de ligne par sa référence
        /// </summary>
        public int Id => Item.Id;
             
        /// <summary>
        /// Référence de la facture de la ligne par sa référence
        /// </summary>
        public int InvoiceId => Item.InvoiceId;

        /// <summary>
        /// Date de la ligne par sa référence
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// Description pour la ligne par sa référence
        /// </summary>
        public string Desc => Item.Desc;

        /// <summary>
        /// Nom du produit pour la ligne  par sa référence
        /// </summary>
        public string ProductName => Item.ProductName;

        /// <summary>
        /// Quantité de produit pour la ligne par sa référence
        /// </summary>
        public double Quantity => Item.Quantity;

        /// <summary>
        /// Prix unitaire du produit pour la ligne par sa référence
        /// </summary>
        public double UnitPrice => Item.UnitPrice;

        /// <summary>
        /// Prix unitaire du produit pour la ligne par sa référence
        /// </summary>
        public double TotalAmount => UnitPrice * Quantity;

        /// <summary>
        /// Largeur des images
        /// </summary>
        public int WidthRequest => Settings.Instance.ImageWidthRequest;

        /// <summary>
        /// Hauteur des images
        /// </summary>
        public int HeightRequest => Settings.Instance.ImageHeightRequest;


        /// <summary>
        /// Reference à la ligne par sa référence
        /// </summary>
        public readonly LineEntity Item;

        protected Line(LineEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Mise à jour à la ligne par sa référence
        /// </summary>
        public void Update(DateTime effectiveOn, string desc, IEnumerable<string> images)
        {


            Item.Desc = desc;
            Item.EffectiveOn= effectiveOn;
            DatabaseAccess.Instance.Update(Item);
        }

        /// <summary>
        /// Suppression des médias associés à la ligne et suppression de la ligne
        /// </summary>
        public void Delete()
        {
            DatabaseAccess.Instance.Remove(Item);
        }
    }
}
