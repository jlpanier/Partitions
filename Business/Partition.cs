using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des partitions 
    /// </summary>
    public class Partition 
    {
        public static List<Partition> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<Partition>();
                    var items = DatabaseAccess.Instance.GetPartitions();
                    foreach (var item in items)
                    {
                        _all.Add(new Partition(item));
                    }
                }
                return _all;
            }
        }
        private static List<Partition>? _all;

        public static Partition Create(string[] files)
        {
            var sources = new List<Source>();
            foreach(string file in files)
            {
                sources.Add(Source.From(file));
            }
            var source = sources.FirstOrDefault(_ => _.Type  == Source.SourceType.MuseScore);
            if (source == null)
            {
                source = sources.FirstOrDefault(_ => _.Type == Source.SourceType.Conducteur);
                if (source == null)
                {
                    source = sources.FirstOrDefault(_ => _.Type == Source.SourceType.PDF);
                }
            }

            if (source==null)
            {
                throw new Exception("Unable to determine source for the partition.");
            }
            var item = new PieceEntity
            {
                EffectiveOn = source.EffectiveOn,
                Label = source.Name,
                Compositor = string.Empty,
                Arranger = string.Empty,
                Duration = 0
            };

            DatabaseAccess.Instance.Insert(item);
            var result = new Partition(item);
            All.Add(result);

            sources.ForEach(_ =>
            {
                _.Add(item.Id);
            });
            return result;
        }

        /// <summary>
        /// Référence de la partition
        /// </summary>
        public int Id => Item.Id;
             
        /// <summary>
        /// Date de la partition
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// Nom de la partition
        /// </summary>
        public string Label => Item.Label;

        /// <summary>
        /// Compositeur de la partition
        /// </summary>
        public string Compositor => Item.Compositor;

        /// <summary>
        /// Arrangeur de la partition
        /// </summary>
        public string Arranger => Item.Arranger;

        /// <summary>
        /// Durée en seconde de la partition
        /// </summary>
        public int Duration => Item.Duration;

        /// <summary>
        /// Fichiers associés
        /// </summary>
        public IEnumerable<Source> Sources => Source.All.Where(_ => _.PieceId == Id);

        /// <summary>
        /// Reference de la partition
        /// </summary>
        public readonly PieceEntity Item;

        protected Partition(PieceEntity item)
        {
            Item = item;
        }

        public void Save()
        {
            try
            {
                if (Item.Id == 0)
                {
                    DatabaseAccess.Instance.Insert(Item);
                }
                else
                {
                    DatabaseAccess.Instance.Update(Item);
                }
                Sources.ToList().ForEach(_ =>
                {
                    _.Save();
                });

            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to save partition {Item.Label} : {ex.Message}");
            }
        }

        /// <summary>
        /// Mise à jour de la partition
        /// </summary>
        public void Update(DateTime effectiveOn, string desc, IEnumerable<string> images)
        {
            Item.Compositor = desc;
            Item.EffectiveOn= effectiveOn;
            DatabaseAccess.Instance.Update(Item);
        }

        /// <summary>
        /// Suppression de la partition
        /// </summary>
        public void Delete()
        {
            DatabaseAccess.Instance.Remove(Item);
        }
    }
}
