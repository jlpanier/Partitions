namespace Business
{
    /// <summary>
    /// Gestion des fichiers sources (PDF, MuseScore, SND) des partitions
    /// </summary>
    public class Source
    {
        /// <summary>
        /// Nature du fichier: PDF, MuseScore, SND
        /// </summary>
        public enum NatureType
        {
            Unknown,
            MuseScore,
            PDF,
            SND,
            VIDEO
        }

        /// <summary>
        /// Type de fichier: PDF, MuseScore, SND, Soprano, Alto, Tenor, Baryton, Basse
        /// </summary>
        public enum SourceType
        {
            Unknown,
            MuseScore,
            Conducteur,
            Sop,
            Alto,
            Tenor,
            Bar,
            Bass,
            PDF,
            SND,
            VIDEO
        }

        /// <summary>
        /// Nature du fichier: PDF, MuseScore, SND
        /// </summary>
        private static SourceType GetType(string filePath)
        {
            SourceType type;
            switch (Path.GetExtension(filePath))
            {
                case ".mscz":
                    type = SourceType.MuseScore;
                    break;
                case ".pdf":
                    if (filePath.ToLower().Contains("conducteur"))
                        type = SourceType.Conducteur;
                    else if (filePath.ToLower().Contains("parties"))
                        type = SourceType.Conducteur;
                    else if (filePath.ToLower().Contains("sop"))
                        type = SourceType.Sop;
                    else if (filePath.ToLower().Contains("alto"))
                        type = SourceType.Alto;
                    else if (filePath.ToLower().Contains("tenor"))
                        type = SourceType.Tenor;
                    else if (filePath.ToLower().Contains("bar"))
                        type = SourceType.Bar;
                    else if (filePath.ToLower().Contains("bass"))
                        type = SourceType.Bass;
                    else type = SourceType.PDF;
                    break;
                case ".mp3":
                case ".wav":
                    type = SourceType.SND;
                    break;
                case ".m4a":
                    type = SourceType.VIDEO;
                    break;
                default:
                    type = SourceType.Unknown;
                    break;
            }
            return type;
        }

        /// <summary>
        /// Conversion d'un chemin de fichier en objet Source
        /// </summary>
        public static Source From(string filePath) => new Source(filePath);

        private Source(string filepath)
        {
            FilePath = filepath;
        }

        /// <summary>
        /// Fichier source (PDF, MuseScore, SND) de la partition
        /// </summary>
        public readonly string FilePath;

        /// <summary>
        /// Nom du fichier
        /// </summary>
        public string FileName => Path.GetFileNameWithoutExtension(FilePath);

        /// <summary>
        /// Répertoire du fichier
        /// </summary>
        public string Folder => Path.GetDirectoryName(FilePath) ?? "";

        /// <summary>
        /// Date de création du fichier
        /// </summary>
        public DateTime CreatedOn => System.IO.File.GetCreationTime(FilePath);

        /// <summary>
        /// Nature du fichier: PDF, MuseScore, SND
        /// </summary>
        public SourceType Type => GetType(FilePath);

        /// <summary>
        /// Nature du fichier: PDF, MuseScore, SND
        /// </summary>
        public NatureType Nature
        {
            get
            {
                var result = NatureType.Unknown;
                switch (Type)
                {
                    case SourceType.Conducteur:
                    case SourceType.Sop:
                    case SourceType.Alto:
                    case SourceType.Tenor:
                    case SourceType.Bar:
                    case SourceType.Bass:
                    case SourceType.PDF:
                        result = NatureType.PDF;
                        break;
                    case SourceType.SND:
                        result = NatureType.SND;
                        break;
                    case SourceType.Unknown:
                        break;
                    case SourceType.MuseScore:
                        result = NatureType.MuseScore;
                        break;
                    case SourceType.VIDEO:
                        result = NatureType.VIDEO;
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// Unicode représentant le type de fichier: PDF, MuseScore, SND
        /// </summary>
        public string Unicode
        {
            get
            {
                var result = "";
                switch (Type)
                {
                    case SourceType.Sop:
                    case SourceType.Alto:
                    case SourceType.Tenor:
                    case SourceType.Bar:
                    case SourceType.Bass:
                        result = "🎷";
                        break;
                    case SourceType.Conducteur:
                    case SourceType.PDF:
                        result = "📄";
                        break;
                    case SourceType.SND:
                        result = "🎵";
                        break;
                    case SourceType.VIDEO:
                        result = "🎬";
                        break;
                    case SourceType.Unknown:
                        result = "❓";
                        break;
                    case SourceType.MuseScore:
                        result = "🎼";
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// VRAI, si le fichier est archivé
        /// </summary>
        public bool IsArchived => FileName.StartsWith("_");

        /// <summary>
        /// Renommage le fichier source
        /// </summary>
        public void Rename(string newName)
        {
            var newFilePath = Path.Combine(Folder, newName + Path.GetExtension(FilePath));
            if (System.IO.File.Exists(newFilePath))
            {
                throw new Exception($"Le fichier {newFilePath} existe déjà.");
            }
            System.IO.File.Move(FilePath, newFilePath);
        }

        /// <summary>
        /// Archive le fichier source
        /// </summary>
        public void Archive()
        {
            string newFilePath = "";
            if (IsArchived)
            {
                newFilePath = Path.Combine(Folder, Path.GetFileName(FilePath).Substring(1));
            }
            else
            {
                newFilePath = Path.Combine(Folder, $"_{Path.GetFileName(FilePath)}");
            }
            if (System.IO.File.Exists(newFilePath))
            {
                throw new Exception($"Le fichier {newFilePath} existe déjà.");
            }
            else
            {
                System.IO.File.Move(FilePath, newFilePath);
            }
        }

        /// <summary>
        /// Suppression le fichier source
        /// </summary>
        public void Delete()
        {
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
        }
    }
}
