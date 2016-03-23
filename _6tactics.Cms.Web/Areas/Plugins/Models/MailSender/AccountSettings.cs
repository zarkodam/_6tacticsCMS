using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Web.Areas.Plugins.Models.MailSender
{
    public class AccountSettings
    {
        [Required, StringLength(50)]
        public string Username { get; set; }
        [Required, StringLength(50)]
        public string Password { get; set; }
        [Required, EmailAddress, StringLength(80)]
        public string TargetAddress { get; set; }
    }
}