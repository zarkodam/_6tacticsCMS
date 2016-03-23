using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Web.Areas.Plugins.Models.MailSender
{
    public class ValidationMessage : IFormElements
    {
        [Required, StringLength(200)]
        public string Language { get; set; }

        [Required, StringLength(200)]
        public string Email { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required, StringLength(200)]
        public string Subject { get; set; }

        [Required, StringLength(200)]
        public string Content { get; set; }

        [Required, StringLength(200)]
        public string File { get; set; }

        [Required, StringLength(200)]
        public string CaptchaCode { get; set; }

        public ValidationMessage()
        {
            Language = "EN";
            Email = "required as valid e-mail address, 100 character max";
            Name = "required, 100 character max, A-Z a-z";
            Subject = "required, 500 character max";
            Content = "required, 500 character max";
            CaptchaCode = "required as a vaild solution";
            File = "3Mb max, extensions allowed: rar, zip, 7z";
        }
    }
}