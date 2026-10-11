using SQLite;
using System.ComponentModel;

namespace Repository.Entities
{
    [Table("PARTITIONTAGS")]
    public partial class PartitionTagsEntity : BaseEntity, INotifyPropertyChanged
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

        [Column("PartitionId")]
        public int PartitionId
        {
            get { return _partitionId; }
            set
            {
                if (_partitionId != value)
                {
                    _partitionId = value;
                    NotifyPropertyChanged(nameof(PartitionId));
                }
            }
        }
        private int _partitionId = 0;

        [Column("TagId")]
        public int TagId
        {
            get { return _tagId; }
            set
            {
                if (_tagId != value)
                {
                    _tagId = value;
                    NotifyPropertyChanged(nameof(TagId));
                }
            }
        }
        private int _tagId = 0;

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
