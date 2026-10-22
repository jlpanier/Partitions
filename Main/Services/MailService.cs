using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Main.Services
{
    public class MailService: Main.Services.IMailService
    {
        public async Task SendMailAsync(string partitionName, IEnumerable<Business.Mail> mails, IEnumerable<string> pdfFiles)
        {
            if (!mails.Any())
            {
                throw new Exception("Aucun mail sélectionné");
            }
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(Business.Settings.Instance.noreply, Business.Settings.Instance.Email));

            foreach(var mail in mails)
            {
                message.To.Add(new MailboxAddress(mail.Name, mail.Email));
            }

            message.Subject = $"Partition \"{partitionName}\"";

            var builder = new BodyBuilder
            {
                TextBody = $"Envoi de la partition \"{partitionName}\"."
            };

            // Ajout des PDF
            foreach (var pdfFile in pdfFiles)
            {
                if (File.Exists(pdfFile))
                {
                    builder.Attachments.Add(pdfFile);
                }
            }
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();

            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                var cert2 = certificate as X509Certificate2;

                if (cert2 != null &&
                    cert2.Subject.Contains("smtp.gmail.com"))
                {
                    return true;
                }

                return false;
            };

            await client.ConnectAsync(
                "smtp.gmail.com",
                587,
                MailKit.Security.SecureSocketOptions.StartTls);


            // Vérifiez : https://myaccount.google.com/security
            // Dans Connexion à Google, regardez l'état de : Validation en deux étapes
            // https://myaccount.google.com/apppasswords
            await client.AuthenticateAsync(Business.Settings.Instance.Email, Business.Settings.Instance.Pawd); 

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
