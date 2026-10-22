using CommunityToolkit.Maui.Views;
using FFImageLoading.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Main.Pages;

public partial class MailPopup : Popup
{
    /// <summary>
    /// Nom 
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Email 
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// VRAI, si associé 
    /// </summary>
    public bool IsSuccess { get; private set; } = false;

    /// <summary>
    /// VRAI, si supprimée
    /// </summary>
    public bool IsDeleted { get; private set; } = false;

    public MailPopup(string name = "", string email = "")
	{
        Name = name;
        Email = email;
        InitializeComponent();
        BindingContext = this;
    }

    /// <summary>
    /// Annulation
    /// </summary>
    private async void OnCancel(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    /// <summary>
    /// Sauvegarde de l'email 
    /// </summary>
    private async void OnSave(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new Exception("Le nom est incorrect.");
            }
            var attribute = new EmailAddressAttribute();
            if (string.IsNullOrWhiteSpace(Email) || !attribute.IsValid(Email))
            {
                throw new Exception("L'adresse email n'est pas valide.");
            }

            var mail = Business.Mail.All.FirstOrDefault(_ => _.Email == Email);
            if (mail == null)
            {
                Business.Mail.Create(Name, Email);
            }
            else
            {
                mail.Update(Name, Email);
            }
            IsSuccess = true;
            await CloseAsync();
        }
        catch (Exception ex)
        {
            await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
        }
    }

    /// <summary>
    /// Suppression de l'email 
    /// </summary>
    private async void OnDelete(object sender, EventArgs e)
    {
        try
        {
            var mail = Business.Mail.All.FirstOrDefault(_ => _.Email == Email);
            if (mail != null)
            {
                mail.Delete();
            }
            IsDeleted = true;
            await CloseAsync();

        }
        catch (Exception ex)
        {
            await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
        }
    }
}