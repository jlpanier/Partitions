using CommunityToolkit.Maui.Views;

namespace Main.Pages;

public partial class FileActionPopup : Popup
{
    /// <summary>
    /// Nom du fichier sur lequel l'action est effectuée
    /// </summary>
    public string FileName { get; set; } = "";

    /// <summary>
    /// VRAI, si le fichier est renommé
    /// </summary>
    public bool IsRename { get; private set; } = false;

    /// <summary>
    /// VRAI, si le fichier a été supprimé
    /// </summary>
    public bool IsDelete { get; private set; } = false;

    /// <summary>
    /// VRAI, si le fichier a été archivé
    /// </summary>
    public bool IsArchive { get; private set; } = false;

    public FileActionPopup(string fileName)
	{
		FileName = fileName;
		InitializeComponent();
        BindingContext = this;
    }

    private async void OnCancel(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    private async void OnSave(object sender, EventArgs e)
    {
        IsRename = true;
        await CloseAsync();
    }

    private async void OnDelete(object sender, EventArgs e)
    {
        IsDelete = true;
        await CloseAsync();
    }

    private async void OnArchive(object sender, EventArgs e)
    {
        IsArchive = true;
        await CloseAsync();
    }
}