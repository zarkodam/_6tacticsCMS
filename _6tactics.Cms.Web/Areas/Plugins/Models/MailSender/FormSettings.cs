namespace _6tactics.Cms.Web.Areas.Plugins.Models.MailSender
{
    public class FormSettings
    {
        public bool IsCaptchaEnabled { get; set; }
        public bool IsUploadEnabled { get; set; }

        public FormSettings()
        {
            IsCaptchaEnabled = true;
            IsUploadEnabled = false;
        }
    }
}