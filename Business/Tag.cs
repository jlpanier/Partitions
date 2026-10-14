using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des factures
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Affichage par défaut des factures
        /// </summary>
        public override string ToString() => Label;

        /// <summary>
        /// Toutes les factures
        /// </summary>
        public static List<Tag> All
        {
            get
            {
                if (_tags == null)
                {
                    _tags = new List<Tag>();
                    var items = DatabaseAccess.Instance.GetTags();
                    foreach (var item in items)
                    {
                        _tags.Add(new Tag(item));
                    }
                }
                return _tags;
            }
        }
        private static List<Tag>? _tags = null;

        /// <summary>
        /// Recherche une facture par son identifiant
        /// </summary>
        public static Tag? GetById(int id) => All.FirstOrDefault(i => i.Id == id);

        /// <summary>
        /// Créer une facture de maintenance de travail
        /// </summary>
        public static Tag Create(string label)
        {
            var item = new TagsEntity
            {
                Label = label,
                DateMaj = DateTime.Now,
            };
            DatabaseAccess.Instance.Add(item);

            var result = new Tag(item);
            Tag.All.Add(result);
            return result;
        }

        /// <summary>
        /// Reference de la facture
        /// </summary>
        public int Id => Item.Id;

        /// <summary>
        /// Origine de la facture
        /// </summary>
        public string Label => Item.Label;

        /// <summary>
        /// Reference de l'entity de la facture
        /// </summary>
        public readonly TagsEntity Item;

        /// <summary>
        /// constructeur privé pour créer une instance de facture à partir d'une entity
        /// </summary>
        private Tag(TagsEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Mise à jour de la facture
        /// </summary>
        public void Update(string label)
        {
            Item.Label = label;
            Item.DateMaj = DateTime.Now;
            DatabaseAccess.Instance.Update(Item);
        }

        /// <summary>
        /// Suppression de la facture
        /// </summary>
        public void Delete()
        {
            // Vérifier que le tag n'est pas utilisé par des partitions
            All.Remove(this);
            DatabaseAccess.Instance.Remove(Item);
        }

    }

}
