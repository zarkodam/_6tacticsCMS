using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Web.Areas.Plugins.Models.MailSender
{
    public class InputTitleSettings : IFormElements
    {
        [Required, StringLength(50)]
        public string Language { get; set; }

        [Required, EmailAddress, StringLength(80)]
        public string Email { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Subject { get; set; }

        [Required, StringLength(50)]
        public string Content { get; set; }

        [Required, StringLength(50)]
        public string File { get; set; }

        [Required, StringLength(50)]
        public string CaptchaCode { get; set; }

        [Required, StringLength(50)]
        public string UploadButton { get; set; }

        [Required, StringLength(50)]
        public string SendButton { get; set; }

        public InputTitleSettings()
        {
            Language = "EN";
            Email = "Email";
            Name = "Name";
            Subject = "Subject";
            Content = "Content";
            CaptchaCode = "CaptchaCode";
            File = "File";
            UploadButton = "UploadButton";
            SendButton = "SendButton";
        }
    }
}