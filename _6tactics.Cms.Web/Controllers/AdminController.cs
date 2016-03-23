using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Utilities;
using _6tactics.Cms.Core.ViewModels.Admin;
using _6tactics.Cms.Services.Admin;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    [Authorizer(Roles = "Administrators,AdminReadOnly")]
    public class AdminController : Controller
    {

        #region Fields

        private readonly IAdminLogicService _adminLogicService;
        private readonly IAdminViewModelBuilderService _adminViewModelBuilderService;

        #endregion


        #region Constructors

        public AdminController(
            IAdminLogicService adminService,
            IAdminViewModelBuilderService adminViewModelBuilderService)
        {
            _adminLogicService = adminService;
            _adminViewModelBuilderService = adminViewModelBuilderService;
        }

        #endregion


        #region Index

        public ActionResult Index(string lang, int? pageId)
        {
            return CreateIndexAndPrioritiesView(lang, pageId);
        }


        #endregion


        //#region Details

        //public ActionResult Details(int? id)
        //{
        //    AdminActionsViewModel viewModel = _adminViewModelBuilderService.CreateAdminActionsViewModel(id, ContentItemAction.Details);

        //    if (id == null || viewModel.ContentItemModel == null) return RedirectToAction("Index");

        //    return View(viewModel);
        //}

        //#endregion


        #region Create

        public ActionResult Create(int? parentId, string language = null)
        {
            if (parentId == null) return RedirectToAction("Index");

            AdminActionsViewModel viewModel = _adminViewModelBuilderService.CreateAdminActionsViewModel(parentId, ContentItemAction.Create, language);

            return View(viewModel);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public ActionResult Create(AdminActionsViewModel model, HttpPostedFileBase file)
        {
            if (model.ContentItemViewModel.LinkOption != LinkOption.Custom)
                ModelState.Remove("ContentItemViewModel.LinkTo");

            // Checking is modelstate valid
            if (!ModelState.IsValid)
            {
                PopupMessageUtility.SetMessage(ContentItemAction.ModelState, MessageType.Error);

                return View(_adminViewModelBuilderService.CreateAdminActionsViewModel(model.ContentItemViewModel.ParentId,
                         ContentItemAction.Create, model.CreateEditDataViewModel.ElementLanguage));
            }

            // Check if child element with same name under current language exist
            if (_adminLogicService.IsChildElemetnWithSameNameExist(model) || _adminLogicService.IsPageWithSameNameUnderSelectedLanguageExist(model))
            {
                PopupMessageUtility.SetMessage(ContentItemAction.AlreadyExistInDbOnCreate, MessageType.Warning);

                return View(_adminViewModelBuilderService.CreateAdminActionsViewModel(model.ContentItemViewModel.ParentId,
                            ContentItemAction.AlreadyExistInDbOnCreate, model.CreateEditDataViewModel.ElementLanguage));
            }

            // Check if footer under current language exist
            if (_adminLogicService.IsFooterUnderSameLanguageExist(model))
            {
                PopupMessageUtility.SetMessage(ContentItemAction.FooterUnderLanguageAlreadyExistOnCreate, MessageType.Warning);

                return View(_adminViewModelBuilderService.CreateAdminActionsViewModel(model.ContentItemViewModel.ParentId,
                            ContentItemAction.FooterUnderLanguageAlreadyExistOnCreate, model.CreateEditDataViewModel.ElementLanguage));
            }

            ContentItem contentItem = _adminLogicService.MapAndSaveAndReturnContentItem(model);

            var redirectionId = _adminLogicService.GetRedirectionId(model, contentItem);

            if (redirectionId != null)
            {
                PopupMessageUtility.SetMessage(ContentItemAction.Create, MessageType.Success);
                return RedirectToAction("Create", new { parentId = redirectionId });
            }

            PopupMessageUtility.SetMessage(ContentItemAction.Create, MessageType.Success);


            return RedirectToAction("Index");
        }

        #endregion


        #region Edit

        public ActionResult Edit(int? id, string language = null)
        {
            if (id == null) return RedirectToAction("Index");

            AdminActionsViewModel viewModel = _adminViewModelBuilderService.CreateAdminActionsViewModel(id, ContentItemAction.Edit, language);

            if (viewModel.ContentItemViewModel == null) return RedirectToAction("Index");

            return View(viewModel);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public ActionResult Edit(AdminActionsViewModel adminActionsViewModel, HttpPostedFileBase file)
        {
            var model = adminActionsViewModel;

            if (model.ContentItemViewModel.LinkOption != LinkOption.Custom)
                ModelState.Remove("ContentItemViewModel.LinkTo");

            // Checking is modelstate valid
            if (!ModelState.IsValid)
            {
                PopupMessageUtility.SetMessage(ContentItemAction.ModelState, MessageType.Error);

                return View(_adminViewModelBuilderService.CreateAdminActionsViewModel(model.ContentItemViewModel.Id,
                         ContentItemAction.Edit, model.CreateEditDataViewModel.ElementLanguage));
                //return RedirectToAction("Index");
            }

            // Check if child element with same name under current language exist
            if (_adminLogicService.IsChildElemetnWithSameNameExist(model) || _adminLogicService.IsPageWithSameNameUnderSelectedLanguageExist(model))
            {
                PopupMessageUtility.SetMessage(ContentItemAction.AlreadyExistInDbOnEdit, MessageType.Warning);

                return View(_adminViewModelBuilderService.CreateAdminActionsViewModel(model.ContentItemViewModel.Id,
                            ContentItemAction.AlreadyExistInDbOnEdit, model.CreateEditDataViewModel.ElementLanguage));
            }

            // Check if footer under current language exist
            if (_adminLogicService.IsFooterUnderSameLanguageExist(model))
            {
                PopupMessageUtility.SetMessage(ContentItemAction.FooterUnderLanguageAlreadyExistOnEdit, MessageType.Warning);

                return View(_adminViewModelBuilderService.CreateAdminActionsViewModel(model.ContentItemViewModel.Id,
                            ContentItemAction.FooterUnderLanguageAlreadyExistOnEdit, model.CreateEditDataViewModel.ElementLanguage));
            }

            _adminLogicService.UpdateContentItemAndHisChildren(adminActionsViewModel);

            // Fires success message
            PopupMessageUtility.SetMessage(ContentItemAction.Edit, MessageType.Success);

            return View(_adminViewModelBuilderService.CreateAdminActionsViewModel(model.ContentItemViewModel.Id,
                            ContentItemAction.Edit, model.CreateEditDataViewModel.ElementLanguage));

            //return RedirectToAction("Index");
        }

        #endregion


        #region Priorities

        public ActionResult Priorities(string lang, int? pageId)
        {
            return CreateIndexAndPrioritiesView(lang, pageId);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ActionName("Priorities"), ValidateAntiForgeryToken]
        public ActionResult SetPriorities([Bind(Include = "Id,Priority")] FormCollection formItem)
        {
            if (!ModelState.IsValid)
            {
                PopupMessageUtility.SetMessage(ContentItemAction.ModelState, MessageType.Error);
                return RedirectToAction("Index");
            }

            _adminLogicService.SetPriorities(formItem);

            PopupMessageUtility.SetMessage(ContentItemAction.Priorities, MessageType.Success);
            return RedirectToAction("Index");
        }

        #endregion


        #region Delete

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dataForDelete = _adminLogicService.GetDataForDelete(id);
            if (dataForDelete == null) return HttpNotFound();

            return PartialView(dataForDelete);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            _adminLogicService.DeleteContentItems(id);

            PopupMessageUtility.SetMessage(ContentItemAction.Delete, MessageType.Success);
            return RedirectToAction("Index");
        }

        #endregion


        #region Dispose

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //}

        #endregion


        #region Utilities

        private ActionResult CreateIndexAndPrioritiesView(string language, int? pageId)
        {
            AdminActionsViewModel viewModel = _adminViewModelBuilderService
                .CreateAdminActionsViewModel(language, pageId, Server.MapPath("~/Content/Uploads"));

            if (viewModel.ContentItemModel != null) return View(viewModel);

            PopupMessageUtility.SetMessage(ContentItemAction.NewProject, MessageType.Info);
            return RedirectToAction("Index", "Setup");
        }

        #endregion 

    }

}
