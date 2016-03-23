using _6tactics.Cms.Web.Areas.Plugins.Models.MailSender;

namespace _6tactics.Cms.Web.Areas.Plugins.ViewModels.MailSender
{
    public class MailFormViewModel
    {
        public InputTitleSettings InputTitleSettings { get; set; }
        public MailForm MailForm { get; set; }
        public ValidationMessage ValidationMessage { get; set; }
        public FormSettings FormSettings { get; set; }
    }
}