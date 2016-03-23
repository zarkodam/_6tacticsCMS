using _6tactics.SimpleCaptcha;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace _6tactics.Cms.Web.Areas.Plugins.Models.MailSender
{
    public class MailForm
    {
        [Required, RegularExpression("^[A-Z a-z]+$"), StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required, RegularExpression("^[A-Z a-z]+$"), StringLength(100)]
        public string Subject { get; set; }

        [Required, StringLength(500)]
        public string Content { get; set; }

        [Display(Name = "Captcha")]
        public string MailFormCaptcha { get; set; }

        [Required(ErrorMessage = "You have to solve the problem!")]
        [Display(Name = "Calculate")]
        [SimpleCaptchaValidator("MailFormCaptcha", ErrorMessage = "Result is not valid!")]
        public string CaptchaCode { get; set; }

        //[Required, MaxFileSize(3, ErrorMessage = "Maximum allowed file size is {0} Mb")]
        public HttpPostedFileBase File { get; set; }
    }
}