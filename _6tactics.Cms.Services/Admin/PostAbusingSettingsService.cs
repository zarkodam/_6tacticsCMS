using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Models.Admin;
using _6tactics.Cms.Services.Plugins;

namespace _6tactics.Cms.Services.Admin
{
    public class PostAbusingSettingsService : IPostAbusingSettingsService
    {
        private readonly IPluginsManagerService _pluginsManager;

        public PostAbusingSettingsService(IPluginsManagerService pluginsManager)
        {
            _pluginsManager = pluginsManager;
        }

        public IPostAbusingSettings Get(PostAbusingSettingsFor chooseSetting)
        {
            string settingsName = chooseSetting.Equals(PostAbusingSettingsFor.MailForm)
                ? "mailFormPostAbusingSettings"
                : "loginFormPostAbusingSettings";

            return _pluginsManager.GetPluginData<PostAbusingSettings>("PostAbusing", "mailFormPostAbusingSettings") ?? new PostAbusingSettings();
        }
    }
}
