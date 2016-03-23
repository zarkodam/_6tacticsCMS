using _6tactics.Cms.Core.Models.MailSender;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.ViewModels.MailSender
{
    public class MailSenderSettingsViewModel
    {
        public MailSenderInputNameSettings MailSenderSettings { get; set; }
        public List<MailSenderInputNameSettings> MailSenderSettingsList { get; set; }
    }
}