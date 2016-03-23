using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Services.Plugins;
using _6tactics.Cms.Web.Areas.Plugins.Models.GoogleAnalytics;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Areas.Plugins.Controllers
{
    public class GoogleAnalyticsController : Controller
    {
        private static GoogleAnalyticsSetup _savedSetup;
        private readonly IPluginsManagerService _pluginsManager;
        public GoogleAnalyticsController(IPluginsManagerService pluginsManager)
        {
            _pluginsManager = pluginsManager;

            var googleAnalyticsSetup = _pluginsManager.GetPluginData<GoogleAnalyticsSetup>("GoogleAnalytics", "googleAnalyticsSetup");
            _savedSetup = googleAnalyticsSetup ?? new GoogleAnalyticsSetup();
        }

        [ChildActionOnlyCustom, ExcludeFromPluginActionList]
        public PartialViewResult IncludeAnalytics()
        {
            return PartialView(_savedSetup);
        }

        [AjaxOnly]
        public ActionResult Setup()
        {
            return View(_savedSetup);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void Setup(GoogleAnalyticsSetup setup)
        {
            _savedSetup = setup;
            _pluginsManager.SavePluginData("GoogleAnalytics", "googleAnalyticsSetup", setup);
        }
    }
}