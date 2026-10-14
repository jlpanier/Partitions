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
        /// Chargement du journal de board
        /// </summary>
        public IEnumerable<PartitionsEntity> GetPartitions()
        {
            lock (dbLock)
            {
                return Db.Query<PartitionsEntity>("SELECT * FROM PIECES");
            }
        }

        /// <summary>
        /// Chargement du journal de board
        /// </summary>
        public IEnumerable<TagsEntity> GetTags()
        {
            lock (dbLock)
            {
                return Db.Query<TagsEntity>("SELECT * FROM TAGS");
            }
        }

        /// <summary>
        /// Chargement du journal de board
        /// </summary>
        public IEnumerable<PartitionTagsEntity> GetPartitionTags()
        {
            lock (dbLock)
            {
                return Db.Query<PartitionTagsEntity>("SELECT * FROM PARTITIONTAGS");
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

        public int DeleteTags(int partitionId)
        {
            lock (dbLock)
            {
                return Db.Execute("DELETE FROM PARTITIONTAGS WHERE PartitionId = ?", partitionId);
            }
        }
    }
}
