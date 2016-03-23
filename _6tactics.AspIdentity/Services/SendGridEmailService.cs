using Microsoft.AspNet.Identity;
using SendGrid;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace _6tactics.AspIdentity.Services
{
    public class SendGridEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return MailConfiguration(message);
        }

        private async Task MailConfiguration(IdentityMessage message)
        {
            // Getting mail settings from app.config
            var mailSettings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            var credentials = new NetworkCredential(mailSettings.Network.UserName, mailSettings.Network.Password);

            var host = mailSettings.Network.Host.Split(',');
            var hostEmail = host[0];
            var hostName = host[1];

            // Message content definition
            string text = $"Please click on this link to {message.Subject}: {message.Body} \n \n Thanks, \n" + hostName;
            string html = "Please confirm your account by clicking this link: <a href=\"" + message.Body + "\">link</a><br>Thanks,<br><br>" + hostName;

            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new MailAddress(hostEmail, hostName);
            myMessage.Subject = message.Subject;
            myMessage.Text = text;
            myMessage.Html = html;

            // Create an Web transport for sending email.
            var transportWeb = new SendGrid.Web(credentials);

            // Send the email.
            await transportWeb.DeliverAsync(myMessage);
        }
    }
}