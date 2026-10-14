using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace Main.Pages;

public partial class TagPopup : Popup
{
    public ICommand ClickSave => new Command(OnSave);

    public ICommand ClickCancel => new Command(OnCancel);

    public ICommand ClickDelete => new Command(OnDelete);

    public string NewName { get; set; } = "";

    public bool ShouldRename { get; private set; } = false;

    public bool ShouldDelete { get; private set; } = false;

    public TagPopup(string currentName)
    {
        InitializeComponent();
        NewName = currentName;
        BindingContext = this;
    }

    async void OnSave()
    {
        ShouldRename = true;
        await CloseAsync();
    }

    async void OnCancel()
    {
        await CloseAsync();
    }


    async void OnDelete()
    {
        ShouldDelete = true;
        await CloseAsync();
    }
}