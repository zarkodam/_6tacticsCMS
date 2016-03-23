using _6tactics.Cms.Web.Areas.Plugins.Models.MailSender;
using System.Collections.Generic;

namespace _6tactics.Cms.Web.Areas.Plugins.ViewModels.MailSender
{
    public class ValidationsViewModel
    {
        public IEnumerable<string> LanguageList { get; set; }
        public FormSettings FormSettings { get; set; }
        public List<InputTitleSettings> InputTitleSettings { get; set; }
        public List<ValidationMessage> ValidationMessages { get; set; }
    }
}