using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace _6tactics.Utilities.Network
{
    public class MailUtility
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string SendMailCc { get; set; }
        public string SendMailBcc { get; set; }

        private readonly string _smtpServer;
        private readonly int _smtpServerPort;
        private readonly string _senderMail;
        private readonly string _senderMailPassword;
        private readonly string _sendMailTo;
        private readonly bool _enableSsl;

        public MailUtility(string smtpServer, int smtpServerPort, bool enableSsl, string senderMail, string senderMailPassword, string sendMailTo)
        {
            _smtpServer = smtpServer;
            _smtpServerPort = smtpServerPort;
            _enableSsl = enableSsl;
            _senderMail = senderMail;
            _senderMailPassword = senderMailPassword;
            _sendMailTo = sendMailTo;
        }

        public void SendEmail()
        {
            var from = new MailAddress(_senderMail);
            var to = new MailAddress(_sendMailTo);

            var client = new SmtpClient
            {
                Host = _smtpServer,
                Port = _smtpServerPort,
                UseDefaultCredentials = false,
                EnableSsl = _enableSsl,
                Credentials = new NetworkCredential(_senderMail, _senderMailPassword)
            };

            var mailMessage = new MailMessage(from, to)
            {
                //IsBodyHtml = true,
                Sender = to,
                Priority = MailPriority.High,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            if (!string.IsNullOrWhiteSpace(Subject))
                mailMessage.Subject = Subject;

            if (!string.IsNullOrWhiteSpace(Content))
                mailMessage.Body = Content;

            if (!string.IsNullOrWhiteSpace(SendMailCc))
                mailMessage.CC.Add(new MailAddress(SendMailCc));

            if (!string.IsNullOrWhiteSpace(SendMailBcc))
                mailMessage.Bcc.Add(new MailAddress(SendMailBcc));

            try
            {
                client.Send(mailMessage);

            }
            catch (SmtpException ex)
            {
                Trace.WriteLine("SmtpException: " + ex.Message);
            }
        }
    }
}