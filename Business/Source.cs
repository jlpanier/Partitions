namespace Business
{
    public class Source
    {
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
        }

        public enum NatureType
        {
            Unknown,
            MuseScore,
            PDF,
            SND,
        }

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
                default:
                    type = SourceType.Unknown;
                    break;
            }
            return type;
        }

        public static Source From(string filePath) => new Source(filePath);

        public Source(string filepath)
        {
            FilePath = filepath;
        }

        public readonly string FilePath;

        public string FileName => Path.GetFileNameWithoutExtension(FilePath);

        public string Folder => Path.GetDirectoryName(FilePath) ?? "";

        public DateTime CreatedOn => System.IO.File.GetCreationTime(FilePath);

        public SourceType Type => GetType(FilePath);


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
                }
                return result;
            }
        }

        public string Unicode
        {
            get
            {
                var result = "";
                switch (Type)
                {
                    case SourceType.Conducteur:
                    case SourceType.Sop:
                    case SourceType.Alto:
                    case SourceType.Tenor:
                    case SourceType.Bar:
                    case SourceType.Bass:
                    case SourceType.PDF:
                        result = "📄";
                        break;
                    case SourceType.SND:
                        result = "🎵";
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

        public void Rename(string newName)
        {
            var newFilePath = Path.Combine(Folder, newName + Path.GetExtension(FilePath));
            if (System.IO.File.Exists(newFilePath))
            {
                throw new Exception($"Le fichier {newFilePath} existe déjà.");
            }
            System.IO.File.Move(FilePath, newFilePath);
        }
    }
}
