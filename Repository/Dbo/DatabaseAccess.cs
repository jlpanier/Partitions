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
        public IEnumerable<SourceEntity> GetSources()
        {
            lock (dbLock)
            {
                return Db.Query<SourceEntity>("SELECT * FROM SOURCES");
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
    }
}
