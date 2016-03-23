using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace _6tactics.AspIdentity.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return MailConfiguration(message);
        }

        private async Task MailConfiguration(IdentityMessage message)
        {
            // Message content definition
            string text = $"Please click on this link to {message.Subject}: {message.Body}";
            string html = "Please confirm your account by clicking this link: <a href=\"" + message.Body + "\">link</a><br/>";

            // Getting mail settings from app.config
            var mailSettings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            // Message
            var mailMessage = new MailMessage
            {
                From = new MailAddress(mailSettings.Network.UserName),
                Subject = message.Subject
            };

            mailMessage.To.Add(message.Destination);
            mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));



            // Client
            using (var client = new SmtpClient
            {
                Host = mailSettings.Network.Host,
                Port = mailSettings.Network.Port,
                UseDefaultCredentials = false,
                EnableSsl = mailSettings.Network.EnableSsl,
                Credentials = new NetworkCredential(mailSettings.Network.UserName, mailSettings.Network.Password)
            })
            {
                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (SmtpException ex)
                {
                    Debug.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }

            await Task.FromResult(1);
        }
    }
}