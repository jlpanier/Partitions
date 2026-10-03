using Repository.Dbo;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Business
{
    public class Source
    {
        public static List<Source> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<Source>();
                    var items = DatabaseAccess.Instance.GetSources();
                    foreach (var item in items)
                    {
                        _all.Add(new Source(item));
                    }
                }
                return _all;
            }
        }
        private static List<Source>? _all;
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

        public static Source From(string filePath)
        {
            var result = new Source(new SourceEntity()
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                CreatedOn = System.IO.File.GetCreationTime(filePath),
                File = filePath,
                Type = (int)(GetType(filePath))
            });
            return result;
        }

        private Source(SourceEntity item)
        {
            Item = item;
        }

        public readonly SourceEntity Item;

        public SourceType Type => Item.Type switch
        {
            1 => SourceType.MuseScore,
            2 => SourceType.Conducteur,
            3 => SourceType.Sop,
            4 => SourceType.Alto,
            5 => SourceType.Tenor,
            6 => SourceType.Bar,
            7 => SourceType.Bass,
            8 => SourceType.PDF,
            9 => SourceType.SND,
            _ => SourceType.Unknown
        };

        public string Name => Item.Name.Replace("_", " ");

        public string FileName => Path.GetFileName(File);

        public DateTime EffectiveOn => Item.CreatedOn;

        public string File => Item.File;

        public int Id => Item.Id;

        public int PieceId => Item.PieceId;

        public void Add(int pieceId)
        {
            if (Item.Id == 0)
            {
                Item.PieceId = pieceId;
                DatabaseAccess.Instance.Insert(Item);
                All.Add(new Source(Item));
            }
            else
            {
                DatabaseAccess.Instance.Update(Item);
            }
        }


        public void Save()
        {
            DatabaseAccess.Instance.Update(Item);
        }
    }
}
