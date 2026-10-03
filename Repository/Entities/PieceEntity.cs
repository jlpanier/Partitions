using SQLite;
using System.ComponentModel;

namespace Repository.Entities
{
    [Table("PIECES")]
    public partial class PieceEntity : BaseEntity, INotifyPropertyChanged
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
        [Column("CreatedOn")]
        public DateTime EffectiveOn
        {
            get { return _effectiveOn; }
            set
            {
                if (_effectiveOn != value)
                {
                    _effectiveOn = value;
                    NotifyPropertyChanged(nameof(EffectiveOn));
                }
            }
        }
        private DateTime _effectiveOn;

        [Column("Name")]
        public string Label
        {
            get { return _label; }
            set
            {
                if (_label != value)
                {
                    _label = value;
                    NotifyPropertyChanged(nameof(Label));
                }
            }
        }
        private string _label = "";

        [Column("File")]
        public string Compositor
        {
            get { return _compositor; }
            set
            {
                if (_compositor != value)
                {
                    _compositor = value;
                    NotifyPropertyChanged(nameof(Compositor));
                }
            }
        }
        private string _compositor = "";

        [Column("Type")]
        public string Arranger
        {
            get { return _arranger; }
            set
            {
                if (_arranger != value)
                {
                    _arranger = value;
                    NotifyPropertyChanged(nameof(Arranger));
                }
            }
        }
        private string _arranger = "";

        [Column("Duration")]
        public int Duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    NotifyPropertyChanged(nameof(Duration));
                }
            }
        }
        private int _duration;

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
