using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Utilities;
using _6tactics.Cms.Core.ViewModels.Admin;
using _6tactics.Cms.Services.Admin;
using System.Web;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    [Authorizer(Roles = "Administrators,AdminReadOnly")]
    public class SetupController : Controller
    {
        #region Fields

        private readonly ISetupService _setupService;

        #endregion


        #region Constructors

        public SetupController(ISetupService setupService)
        {
            _setupService = setupService;
        }

        #endregion

        public ActionResult Index()
        {
            return View(_setupService.BuildSetupViewModel());
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken]
        public ActionResult Create(SetupViewModel setupViewModel, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                PopupMessageUtility.SetMessage(ContentItemAction.ModelState, MessageType.Error);
                return RedirectToAction("Index", "Admin");
            }

            int? id = setupViewModel.ProjectContentItem.Id;
            bool isActionCreate = id == null;
            bool isActionEdit = id != null;

            if (isActionCreate)
            {
                _setupService.SaveSetup(setupViewModel);
                PopupMessageUtility.SetMessage(ContentItemAction.Create, MessageType.Success);
            }

            if (isActionEdit)
            {
                // Update project content item and seo keywords and description
                _setupService.SaveSetup(setupViewModel);

                PopupMessageUtility.SetMessage(ContentItemAction.Edit, MessageType.Success);
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}
