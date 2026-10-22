using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using FFImageLoading.Helpers;
using Main.ViewModels;
using System.Collections.ObjectModel;
using static Business.Source;

namespace Main.Pages;

public partial class SendMailPopup : Popup
{
    /// <summary>
    /// Ensembles des mails
    /// </summary>
    public ObservableCollection<MailViewModel> Emails { get; set; } = new ObservableCollection<MailViewModel>();

    public bool IsSend { get; set; } = false;

    private readonly Business.Partition _partition;

    public SendMailPopup(Business.Partition partition)
	{
        Emails = new ObservableCollection<MailViewModel>(MailViewModel.All);
        _partition = partition;
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnCancel(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    private async void OnSend(object sender, EventArgs e)
    {
        try
        {
            await ServiceHelper.GetService<IMailService>()!.SendMailAsync(_partition.Name, Emails.Where(_ => _.IsSelected).Select(_ => _.Item), _partition.Sources.Where(_ => _.Nature == NatureType.PDF).Select(_=>_.FilePath));
            await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(Business.Settings.Instance.Name, "Mail envoyé avec succès", "Ok");
            await CloseAsync();
        }
        catch (Exception ex)
        {
            await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
        }
    }

    private async void OnAdd(object sender, EventArgs e)
    {
        try
        {
            var popup = new MailPopup();
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            if (result != null && !result.WasDismissedByTappingOutsideOfPopup)
            {
                await CloseAsync();
            }
        }
        catch (Exception ex)
        {
            await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
        }
    }
}