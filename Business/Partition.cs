using Repository.Dbo;
using Repository.Entities;
using System.Xml.Linq;

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


        public static async Task Import(string[] files, IEnumerable<Tag> tags)
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
                    if (source == null)
                    {
                        source = sources.FirstOrDefault(_ => _.Nature == Source.NatureType.PDF);
                        if (source == null)
                        {
                            source = sources.FirstOrDefault(_ => _.Nature == Source.NatureType.SND);
                            if (source == null)
                            {
                                source = sources.FirstOrDefault(_ => _.Nature == Source.NatureType.VIDEO);
                                if (source == null)
                                {
                                    source = sources.FirstOrDefault();
                                }
                            }
                        }
                    }
                }
            }
            if (source==null)
            {
                throw new Exception($"Source inconnue {files.First()}.");
            }

            var directory = Path.GetDirectoryName(source.FilePath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new Exception($"Répertoire incorrecte \"{directory}\" pour {source.FilePath}");
            }
            var directorysource = Path.GetFileName(directory);
            var destinationfolder = System.IO.Path.Combine(Appli.Instance.FilePath, directorysource);
            Directory.CreateDirectory(destinationfolder);

            foreach(string file in files)
            {
                if (!file.StartsWith("."))
                {
                    var destinationfilename = Path.Combine(destinationfolder, Path.GetFileName(file));
                    File.Move(file, destinationfilename, true);
                }
            }
            var name = directorysource.Replace("_", " ");
            var partition = All.FirstOrDefault(_ => _.Name == name);
            if (partition==null)
            {
                var item = new PartitionsEntity
                {
                    EffectiveOn = source.CreatedOn,
                    Name = name,
                    Compositor = string.Empty,
                    Arranger = string.Empty,
                    Duration = 0,
                    Folder = destinationfolder,
                };

                DatabaseAccess.Instance.Insert(item);
                partition = new Partition(item);
                All.Add(partition);
            }

            foreach (var tag in tags)
            {
                PartitionTagsEntity partitiontag = new PartitionTagsEntity
                {
                    DateMaj=DateTime.Now,
                    PartitionId = partition.Id,
                    TagId = tag.Id
                };
                DatabaseAccess.Instance.Insert(partitiontag);
            }
        }

        /// <summary>
        /// Liste des tags liés à cette partition
        /// </summary>
        public List<Tag> Tags
        {
            get
            {
                if (_tags == null)
                {
                    _tags = new List<Tag>();
                    var partitiontags = PartitionTags.All.Where(_ => _.PartitionId == Id);
                    foreach (var partitiontag in partitiontags)
                    {
                        var tag = Tag.All.FirstOrDefault(_ => _.Id == partitiontag.TagId);
                        if (tag!=null) _tags.Add(tag);

                    }
                }
                return _tags;
            }
        }
        private List<Tag>? _tags = null;


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
        public string Name => Item.Name;

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
        /// Chemin du répertoire de la partition
        /// </summary>
        public string Folder => Item.Folder;

        /// <summary>
        /// Fichiers associés
        /// </summary>
        public IEnumerable<Source> Sources
        {
            get
            {
                var result = new List<Source>();
                if (Directory.Exists(Item.Folder) == false)
                {
                    throw new Exception($"Répertoire inexistant \"{Item.Folder}\" pour {Item.Name}");
                }
                var files = Directory.GetFiles(Item.Folder);
                foreach(var file in files)
                {
                    result.Add(Source.From(file));
                }
                return result;
            }
        }

        /// <summary>
        /// Reference de la partition
        /// </summary>
        public readonly PartitionsEntity Item;

        protected Partition(PartitionsEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// VRAI, si la partition contient le tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private bool Contains(Tag tag) => Tags.Any(_=>_.Id == tag.Id);

        /// <summary>
        /// VRAI, si la partition contient au moins un des tags de la liste
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public bool Contains(IEnumerable<Tag> tags) => Tags.Any() && tags.Any(_ => Contains(_));

        /// <summary>
        /// Savegarde de la partition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="compositeur"></param>
        /// <param name="arrangeur"></param>
        /// <param name="duration"></param>
        /// <param name="tags"></param>
        /// <exception cref="Exception"></exception>
        public void Save(string name, string compositeur, string arrangeur, TimeSpan duration, IEnumerable<Tag> tags)
        {
            try
            {
                if (Name != name)
                {
                    char[] invalid = System.IO.Path.GetInvalidFileNameChars();
                    var newfolder = new string(name.ToUpperInvariant().Select(c => invalid.Contains(c) ? ' ' : c).ToArray());
                    if (string.IsNullOrWhiteSpace(newfolder))
                    {
                        throw new Exception($"Nom incorrecte \"{name}\"!");
                    }
                    var destinationfolder = System.IO.Path.Combine(Appli.Instance.FilePath, newfolder);
                    if (!Directory.Exists(destinationfolder))
                    {
                        Directory.Move(Folder, newfolder);
                        Item.Folder = destinationfolder;
                    }
                    Item.Name = name;
                }

                Item.Compositor = compositeur;
                Item.Arranger = arrangeur;
                Item.Duration = (int)duration.TotalSeconds;

                if (Item.Id == 0)
                {
                    DatabaseAccess.Instance.Insert(Item);
                }
                else
                {
                    DatabaseAccess.Instance.Update(Item);
                }
                PartitionTags.Link(Item.Id, tags);
                _tags = new List<Tag>(tags);

            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to save partition {Item.Name} : {ex.Message}");
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
        /// Mise à jour de la partition
        /// </summary>
        public void SetDuration(TimeSpan ts)
        {
            Item.Duration = (int)ts.TotalSeconds;
            DatabaseAccess.Instance.Update(Item);
        }

        /// <summary>
        /// Suppression de la partition
        /// </summary>
        public void Delete()
        {
            if (Directory.Exists(Folder))
            {
                Directory.Delete(Folder, true);
            }
            All.Remove(this);
            DatabaseAccess.Instance.Remove(Item);
        }

        /// <summary>
        /// Ajout d'un fichier à la partition
        /// </summary>
        public void AddFile(string fullpath)
        {
            if (!Directory.Exists(Folder))
            {
                throw new Exception($"Répertoire inexistant \"{Folder}\"");
            }
            string filename = Path.GetFileName(fullpath);
            string destfile = Path.Combine(Folder, filename);
            if (File.Exists(destfile))
            {
                throw new Exception($"Fichier existant \"{filename}\"");
            }
            File.Copy(fullpath, destfile);
        }
    }
}
