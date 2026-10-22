using Repository.Entities;

namespace Repository.Dbo
{
    /// <summary>
    /// Gestion de la base de données SQLite
    /// </summary>
    public class DatabaseAccess: BaseDbo
    {
        /// <summary>
        /// Instance de la base de données SQLite
        /// </summary>
        public static DatabaseAccess Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null) _instance = new DatabaseAccess();
                    }
                }
                return _instance;
            }
        }
        private static DatabaseAccess? _instance;

        /// <summary>
        /// Lock
        /// </summary>
        private static readonly object _lock = new();

        public DatabaseAccess() : base()
        {
        }

        /// <summary>
        /// Chargement des partitions
        /// </summary>
        public IEnumerable<PartitionsEntity> GetPartitions()
        {
            lock (dbLock)
            {
                return Db.Query<PartitionsEntity>("SELECT * FROM PIECES");
            }
        }

        /// <summary>
        /// Chargement des tags
        /// </summary>
        public IEnumerable<TagsEntity> GetTags()
        {
            lock (dbLock)
            {
                return Db.Query<TagsEntity>("SELECT * FROM TAGS");
            }
        }

        /// <summary>
        /// Chargement des tags liés aux partitions
        /// </summary>
        public IEnumerable<PartitionTagsEntity> GetPartitionTags()
        {
            lock (dbLock)
            {
                return Db.Query<PartitionTagsEntity>("SELECT * FROM PARTITIONTAGS");
            }
        }

        /// <summary>
        /// Suppression des tags liés à une partition
        /// </summary>
        /// <param name="partitionId"></param>
        /// <returns></returns>
        public int DeleteTags(int partitionId)
        {
            lock (dbLock)
            {
                return Db.Execute("DELETE FROM PARTITIONTAGS WHERE PartitionId = ?", partitionId);
            }
        }

        /// <summary>
        /// Chargement de la configuration
        /// </summary>
        public IEnumerable<SettingsEntity> GetSettings()
        {
            lock (dbLock)
            {
                return Db.Query<SettingsEntity>("SELECT * FROM SETTINGS");
            }
        }

        /// <summary>
        /// Chargement des mails
        /// </summary>
        public IEnumerable<MailEntity> GetMails()
        {
            lock (dbLock)
            {
                return Db.Query<MailEntity>("SELECT * FROM MAILS");
            }
        }
    }
}
