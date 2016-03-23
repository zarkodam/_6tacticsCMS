using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Models.Common;
using _6tactics.Cms.Core.ViewModels.Common;
using _6tactics.Cms.Core.ViewModels.Web;
using _6tactics.Cms.Services.Web;
using _6tactics.Cms.Web.App_Logic;
using _6tactics.Utilities.FileSystem;
using _6tactics.Utilities.Network;
using _6tactics.Utilities.StringUtilities;
using _DataAccess.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ContentItem = _6tactics.Cms.Core.Entities.ContentItem;

namespace _6tactics.Cms.Web.Controllers
{
    public abstract class BaseWebController : Controller
    {
        #region Fields

        protected IUnitOfWork Uof;
        protected ISeoDataService SeoDataService;
        protected ISiteMapService SiteMap;


        #endregion


        #region Properties

        private bool IsAllowedByUsersGroup => User.IsInRole("Administrators") || User.IsInRole("AdminReadOnly");
        private Func<ElementVisibility?, bool, bool> IsElementVisible => (v, a) => v == ElementVisibility.Visible || v == ElementVisibility.VisibleForAdminOnly && a == IsAllowedByUsersGroup;

        #endregion


        #region Actions

        //[Route("robots.txt", Name = "GetRobotsText"), OutputCache(Duration = 86400)]
        protected ContentResult RobotsText()
        {
            // https://en.wikipedia.org/wiki/Robots_exclusion_standard
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /error/");
            //stringBuilder.AppendLine("disallow: /admin/");
            //stringBuilder.AppendLine("disallow: /account/");
            //stringBuilder.AppendLine("disallow: /manage/");
            stringBuilder.Append("sitemap: ");
            stringBuilder.AppendLine(Url.RouteUrl("GetSitemapXml", null, Request.Url.Scheme));

            return Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }

        //[Route("sitemap.xml", Name = "GetSitemapXml")]
        protected ContentResult SitemapXml()
        {
            return Content(SiteMap.GetSiteMap(), "text/xml", Encoding.UTF8);
        }

        protected PartialViewResult SeoData()
        {

            return PartialView(SeoDataService.GetSeoData());
        }

        protected ActionResult GetFile(string folderPath, string filename, string extensionName)
        {
            string fileUrl = string.Concat("/", FileSystemUtilities.ChangePathMinusesToForwardSlashes(folderPath), "/", filename, ".", extensionName);

            if (!System.IO.File.Exists(Server.MapPath(fileUrl)))
                return RedirectToAction("notfound", "error");

            DateTime lastModified = System.IO.File.GetLastWriteTime(fileUrl);

            CreateEtag(lastModified);
            CacheData();

            return File(fileUrl, MimeTypeMap.GetMimeType(Path.GetExtension(fileUrl)));
        }

        protected ActionResult Index(string lang, string routeTitle)
        {
            // Getting content items
            var gettingContentItems = new Stopwatch();
            gettingContentItems.Start();

            List<ContentItem> items = Uof.GenericRepo<ContentItem>().Get().ToList();
            gettingContentItems.Stop();
            Trace.WriteLine("TIME OF GETTING CONTENT ITEMS: " + gettingContentItems.ElapsedMilliseconds);

            // If project is not created goto admin
            ContentItem project = items.FirstOrDefault(i => i.ContentType == ContentType.Project);
            if (project == null)
                return RedirectToAction("Index", "Admin", new { area = "" });



            ContentItem selectedLanguage;

            // Selected language
            if (!string.IsNullOrWhiteSpace(lang))
                selectedLanguage = project.ContentItems.FirstOrDefault(i => i.ContentType == ContentType.Language &&
                                    i.Title.Equals(lang, StringComparison.InvariantCultureIgnoreCase) &&
                                    ElementsVisibilityUtility.IsElementVisible(i.ElementVisibility, IsAllowedByUsersGroup));
            // Default language
            else
                selectedLanguage = items.OrderBy(i => i.Priority).FirstOrDefault(i => i.ContentType == ContentType.Language &&
                                    ElementsVisibilityUtility.IsElementVisible(i.ElementVisibility, IsAllowedByUsersGroup));

            // If selectedLanguage is null go create one
            if (selectedLanguage == null)
                return RedirectToAction("Index", "Admin", new { area = "" });

            // Get language children items
            IOrderedEnumerable<ContentItem> languageChildrens = selectedLanguage.ContentItems.OrderBy(x => x.Priority);

            // If there's no page created goto admin and create one
            ContentItem firstOrDefaultPage = languageChildrens.FirstOrDefault(i => i.ContentType == ContentType.Page);
            if (firstOrDefaultPage == null)
                return RedirectToAction("Index", "Setup", new { area = "" });

            // Redirect to language with default page
            if (string.IsNullOrWhiteSpace(routeTitle))
                return RedirectToAction("Index", "Web", new { area = "", lang = selectedLanguage.Title, routeTitle = firstOrDefaultPage.RouteTitle });

            // Current selected page as content item
            ContentItem slectedPageByPageRouteTitle = Uof.ContentItemRepo.GetSelectedPageByRouteTitle(selectedLanguage, routeTitle, IsElementVisible);

            // If slectedPageByPageRouteTitle is null get default page
            ContentItem selectedPage = slectedPageByPageRouteTitle ?? firstOrDefaultPage;

            // Get main menu items
            ContentItem[] pageContent = selectedPage.ContentItems.OrderBy(i => i.Priority).ToArray();

            // Is GoogleMap added to this page(add or remove google maps library)
            bool isGoogleMapAddedToThisPage = pageContent
                .Any(i => i.DisplayType.CustomContains("googlemap", StringComparison.InvariantCultureIgnoreCase));

            // Get footer item
            ContentItem footerItem = selectedLanguage.ContentItems.FirstOrDefault(i => i.ContentType == ContentType.Footer &&
                             ElementsVisibilityUtility.IsElementVisible(i.ElementVisibility, IsAllowedByUsersGroup));

            // Get footer items from all footer elements
            var footerItems = footerItem?.ContentItems
                .Where(i => ElementsVisibilityUtility.IsElementVisible(i.ElementVisibility, IsAllowedByUsersGroup))
                .OrderBy(i => i.Priority)
                .Select(CreateSectionViewModel);



            // Create PageViewModel
            var model = new PageViewModel
            {
                Id = project.Id,
                Title = selectedLanguage.Title,
                Project = new ProjectViewModel
                {
                    Id = project.Id,
                    Title = project.Title,
                    FileUrl = project.FileUrl,
                },
                MainMenu = CreatePageList(selectedLanguage),
                PageContentMenu = CreateSectionViewModel(selectedPage),
                LanguagesWithDefaultRouteTitle = CreateLanguageListWithDefaultRouteTitle(project),
                PageSections = pageContent.Select(CreateSectionViewModel),
                Footer = footerItem,
                FooterSections = footerItems,
                IsAnyGoogleMapOnThisPage = isGoogleMapAddedToThisPage
            };


            return View(model);
        }

        #endregion


        #region Helpers

        private Dictionary<string, string> CreateLanguageListWithDefaultRouteTitle(IContentItem projectContentItem)
        {
            Dictionary<string, string> model = new Dictionary<string, string>();

            foreach (ContentItem language in projectContentItem.ContentItems
                .Where(i => ElementsVisibilityUtility.IsElementVisible(i.ElementVisibility, IsAllowedByUsersGroup)).OrderBy(i => i.Priority))

                if (language.ContentItems.Any(i => i.ContentType == ContentType.Page) && !model.ContainsKey(language.Title))
                    model.Add(language.Title, language.ContentItems.First(i => i.ContentType == ContentType.Page).RouteTitle);

            return model;
        }

        private SectionViewModel CreatePageList(ContentItem item)
        {
            SectionViewModel section = null;

            if (item.ContentItems != null)
            {
                var items = item.ContentItems
                    .Where(i => i.ContentType == ContentType.Page && ElementsVisibilityUtility.IsElementVisible(i.ElementVisibility, IsAllowedByUsersGroup))
                    .OrderBy(i => i.Priority)
                    .Select(CreateContentItemViewModel);

                section = new SectionViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Items = items,
                    ContentType = item.ContentType,
                    DisplayType = item.DisplayType
                };
            }

            return section;
        }

        private SectionViewModel CreateSectionViewModel(ContentItem item)
        {
            if (item == null) return null;
            SectionViewModel section = new SectionViewModel();

            var items = item.ContentItems
            .Where(i => (i.ContentType == ContentType.ContentElement || i.ContentType == ContentType.FileElement || i.ContentType == ContentType.Footer) &&
                     ElementsVisibilityUtility.IsElementVisible(i.ElementVisibility, IsAllowedByUsersGroup))
            .OrderBy(i => i.Priority)
            .Select(CreateContentItemViewModel);

            if (ElementsVisibilityUtility.IsElementVisible(item.ElementVisibility, IsAllowedByUsersGroup))
                section = new SectionViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Items = items,
                    ContentType = item.ContentType,
                    DisplayType = item.DisplayType,
                    FileUrl = item.FileUrl,
                    SectionTitleVisibility = item.SectionTitleVisibility,
                    ElementVisibility = item.ElementVisibility
                };

            return section;
        }

        private ContentItemViewModel CreateContentItemViewModel(ContentItem item)
        {
            Mapper.CreateMap<ContentItem, ContentItemViewModel>();
            var contentItemViewModel = Mapper.Map<ContentItemViewModel>(item);

            return contentItemViewModel;
        }


        private void CreateEtag(DateTime lastModified)
        {
            Response.Cache.SetETag(ShortGuid.NewShortGuid());
            Response.Cache.SetLastModified(lastModified);
        }

        private void CacheData()
        {
            Response.Cache.SetExpires(DateTime.Now.AddDays(10));
            Response.Cache.SetSlidingExpiration(true);
            Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            Response.Cache.SetMaxAge(new TimeSpan(10, 0, 0, 0));
        }
        #endregion
    }
}