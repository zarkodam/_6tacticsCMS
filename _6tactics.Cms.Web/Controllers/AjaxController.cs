using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Helpers;
using _6tactics.Cms.Core.ViewModels.Admin;
using _6tactics.Cms.Services.Common;
using _6tactics.SimpleCaptcha;
using _DataAccess.Repositories;
using System.Linq;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    [AjaxOnly]
    public class AjaxController : Controller
    {
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IContentItemsWithParentCountService _contentItemsWithParentCount;

        public AjaxController(IContentItemRepository contentItemRepository, IContentItemsWithParentCountService contentItemsWithParentCount)
        {
            _contentItemRepository = contentItemRepository;
            _contentItemsWithParentCount = contentItemsWithParentCount;
        }

        public JsonResult GetDisplayTypeList(ContentType selectedContentType)
        {
            return Json(DisplayTypeSelectorHelper.DisplayTypesForEditPage(selectedContentType), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLanguages()
        {
            return Json(_contentItemRepository.LanguageList.ToList(), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PagesBySelectedLanguage(string selectedLanguage, string currentAction)
        {
            return PartialView(new AdminPagesListViewModel
            {
                SelectedLanguage = selectedLanguage,
                CurrentAction = currentAction,
                ElementsDepthAndTitle = _contentItemsWithParentCount
                .GetFiltered(i => i.ContentType == ContentType.Page, _contentItemRepository.GetLanguageItemByTitle(selectedLanguage).ContentItems)
            });
        }

        public JsonResult IsCaptchaValid(string captchaFor, string solution)
        {
            string solutionFixedData = string.IsNullOrWhiteSpace(solution) ? "22" : solution;
            bool isCaptchaValid = SimpleCaptcha.Utilities.IsCaptchaValid(solutionFixedData, SimpleCaptchaInitializer.CurrentCaptcha[captchaFor]);

            return Json(isCaptchaValid, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegenerateCaptcha(string captchaFor)
        {
            return Json(new SimpleCaptchaInitializer().GenerateCaptchaString(captchaFor), JsonRequestBehavior.AllowGet);
        }
    }
}