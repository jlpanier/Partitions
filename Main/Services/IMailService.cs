namespace Main.Services
{
    public interface IMailService
    {
        /// <summary>
        /// Envoi d'un mail
        /// </summary>
        Task SendMailAsync(string partitionName, IEnumerable<Business.Mail> mails, IEnumerable<string> pdfFiles);
    }
}
