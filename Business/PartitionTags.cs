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

        public static void Link(int partitionId, IEnumerable<Tag> tags)
        {
            var item = All.FirstOrDefault(_=>_.PartitionId==partitionId);
            while (item!=null)
            {
                All.Remove(item);
                item = All.FirstOrDefault(_ => _.PartitionId == partitionId);
            }
            int deleted = DatabaseAccess.Instance.DeleteTags(partitionId);
            foreach (var tag in tags)
            {
                var newitem = new PartitionTagsEntity()
                {
                    TagId = tag.Id,
                    DateMaj = DateTime.Now,
                    PartitionId = partitionId,
                };
                DatabaseAccess.Instance.Insert(newitem);
                All.Add(new PartitionTags(newitem));
            }
        }

        /// <summary>
        /// Référence de la partition
        /// </summary>
        public int Id => Item.Id;

        public int PartitionId => Item.PartitionId;

        public int TagId => Item.TagId;

        public readonly PartitionTagsEntity Item;

        public PartitionTags(PartitionTagsEntity item)
        {
            Item = item;
        }
    }
}
