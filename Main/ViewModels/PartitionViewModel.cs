using Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main.ViewModels
{
    public class PartitionViewModel
    {
        public static PartitionViewModel From(Partition item) => new PartitionViewModel(item);

        private PartitionViewModel(Partition item)
        {
            Item = item;
        }

        public readonly Partition Item;

        public int Id => Item.Id;

        public string Label => Item.Label;

        public string Duration => TimeSpan.FromSeconds(Item.Duration).ToString();

        public string Compositor => Item.Compositor;

        public string Arranger => Item.Arranger;

        public DateTime EffectiveOn => Item.EffectiveOn;
    }
}
