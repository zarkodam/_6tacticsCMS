using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Services.Plugins;
using _6tactics.Cms.Web.Areas.Plugins.Models.PostAbusing;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Areas.Plugins.Controllers
{
    public class PostAbusingController : Controller
    {
        private static PostAbusingSettings _mailFormSetup;
        private static PostAbusingSettings _loginFormSetup;
        private readonly IPluginsManagerService _pluginsManager;

        public PostAbusingController(IPluginsManagerService pluginsManager)
        {
            _pluginsManager = pluginsManager;
            var mailFormPostAbusingSettings = _pluginsManager.GetPluginData<PostAbusingSettings>("PostAbusing", "mailFormPostAbusingSettings");
            _mailFormSetup = mailFormPostAbusingSettings ?? new PostAbusingSettings();
            var loginFormPostAbusingSettings = _pluginsManager.GetPluginData<PostAbusingSettings>("PostAbusing", "loginFormPostAbusingSettings");
            _loginFormSetup = loginFormPostAbusingSettings ?? new PostAbusingSettings();
        }

        [AjaxOnly]
        public PartialViewResult MailForm()
        {
            return PartialView(_mailFormSetup);
        }

        [AjaxOnly]
        public PartialViewResult LoginForm()
        {
            return PartialView(_loginFormSetup);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorizer(Roles = "Administrators")]
        public void MailForm(PostAbusingSettings setup)
        {
            _mailFormSetup = setup;
            _pluginsManager.SavePluginData("PostAbusing", "mailFormPostAbusingSettings", setup);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorizer(Roles = "Administrators")]
        public void LoginForm(PostAbusingSettings setup)
        {
            _loginFormSetup = setup;
            _pluginsManager.SavePluginData("PostAbusing", "loginFormPostAbusingSettings", setup);
        }
    }
}