using SQLite;
using System.ComponentModel;

namespace Repository.Entities
{
    [Table("SOURCES")]
    public partial class SourceEntity : BaseEntity, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            IsDirty = true;
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Ignore]
        public bool IsDirty { get; set; }

        #endregion

        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    NotifyPropertyChanged(nameof(Id));
                }
            }
        }
        private int _Id;

        [Indexed]
        [Column("PieceId")]
        public int PieceId
        {
            get { return _pieceId; }
            set
            {
                if (_pieceId != value)
                {
                    _pieceId = value;
                    NotifyPropertyChanged(nameof(PieceId));
                }
            }
        }
        private int _pieceId;


        [Column("CreatedOn")]
        public DateTime CreatedOn
        {
            get { return _effectiveOn; }
            set
            {
                if (_effectiveOn != value)
                {
                    _effectiveOn = value;
                    NotifyPropertyChanged(nameof(CreatedOn));
                }
            }
        }
        private DateTime _effectiveOn;

        [Column("Name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        private string _name = "";

        [Column("File")]
        public string File
        {
            get { return _file; }
            set
            {
                if (_file != value)
                {
                    _file = value;
                    NotifyPropertyChanged(nameof(File));
                }
            }
        }
        private string _file = "";

        [Column("Type")]
        public int Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    NotifyPropertyChanged(nameof(Type));
                }
            }
        }
        private int _type;

        [Column("DateMaj")]
        public DateTime DateMaj
        {
            get { return _datemaj; }
            set
            {
                if (_datemaj != value)
                {
                    _datemaj = value;
                    NotifyPropertyChanged(nameof(DateMaj));
                }
            }
        }
        private DateTime _datemaj;
    }
}
