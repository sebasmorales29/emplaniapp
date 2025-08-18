using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

public class EmailService : IIdentityMessageService
{
    public async Task SendAsync(IdentityMessage message)
    {
        var fromEmail = ConfigurationManager.AppSettings["MailFromEmail"];
        var fromName = ConfigurationManager.AppSettings["MailFromName"];

        using (var mail = new MailMessage())
        {
            mail.From = new MailAddress(fromEmail, fromName);
            mail.To.Add(message.Destination);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            using (var smtp = new SmtpClient()) // lee system.net/mailSettings
            {
                // SmtpClient toma host/puerto/credenciales/SSL del Web.config
                await smtp.SendMailAsync(mail);
            }
        }
    }
}