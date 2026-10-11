using Repository.Dbo;
using Repository.Entities;


namespace Business
{
    public class PartitionTags
    {
        public static List<PartitionTags> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<PartitionTags>();
                    var items = DatabaseAccess.Instance.GetPartitionTags();
                    foreach (var item in items)
                    {
                        _all.Add(new PartitionTags(item));
                    }
                }
                return _all;
            }
        }
        private static List<PartitionTags>? _all;

        /// <summary>
        /// Référence de la partition
        /// </summary>
        public int Id => Item.Id;

        public int PartitionId => Item.PartitionId;

        public int TagId => Item.TagId;

        public readonly PartitionTagsEntity Item;

        private PartitionTags(PartitionTagsEntity item)
        {
            Item = item;
        }
    }
}
