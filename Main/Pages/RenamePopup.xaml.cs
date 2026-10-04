using CommunityToolkit.Maui.Views;
using Main.ViewModels;

namespace Main.Pages;

public partial class RenamePopup : Popup
{
    public string NewName { get; set; } = "";

    public bool IsSuccess { get; private set; } = false;
    public RenamePopup(string currentName)
    {
        InitializeComponent();
        NewName = currentName;
        BindingContext = this;
    }

    async void Cancel_Clicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    void Validate_Clicked(object sender, EventArgs e)
    {
        IsSuccess = true;
        CloseAsync();
    }
}
