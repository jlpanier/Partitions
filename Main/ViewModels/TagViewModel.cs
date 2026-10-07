using Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main.ViewModels
{
    public class TagViewModel:BaseViewModel
    {
        public static List<TagViewModel> All => new List<TagViewModel>(Business.Tag.All.Select(_=> TagViewModel.From(_)));

        public static TagViewModel From(Tag item) => new TagViewModel(item);

        public int Id => Item.Id;

        public string Label
        {
            get => _label;
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

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    IsNotSelected = !_isSelected;
                    NotifyPropertyChanged(nameof(IsSelected));
                }
            }
        }
        private bool _isSelected = false;

        public bool IsNotSelected
        {
            get => _isNotSelected;
            set
            {
                if (_isNotSelected != value)
                {
                    _isNotSelected = value;
                    NotifyPropertyChanged(nameof(IsNotSelected));
                }
            }
        }
        private bool _isNotSelected = true;
        public readonly Tag Item;

        private TagViewModel(Tag item)
        {
            Item = item;
            Label = item.Label;
        }

        public void Select()
        {
            IsSelected = !IsSelected;
        }
    }
}
