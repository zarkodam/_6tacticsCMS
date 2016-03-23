using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Services.Plugins;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    [Authorizer(Roles = "Administrators,AdminReadOnly")]
    public class PluginsManagerController : Controller
    {
        #region Fields

        private readonly IPluginsManagerService _pluginsHandlerService;

        #endregion


        #region Constructors

        public PluginsManagerController(IPluginsManagerService pluginsHandlerService)
        {
            _pluginsHandlerService = pluginsHandlerService;
        }

        #endregion


        public ActionResult Index()
        {
            return View(_pluginsHandlerService.GetListOfPluginsWithActions());
        }


        [AjaxOnly]
        public PartialViewResult PluginActions(string pluginName)
        {
            return PartialView(_pluginsHandlerService.GetPluginActions(pluginName));

        }
    }
}
