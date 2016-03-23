using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Models.Common;
using _6tactics.Cms.Core.ViewModels.Admin;
using _6tactics.Cms.Core.ViewModels.Common;
using _6tactics.Cms.Core.ViewModels.Web;
using _6tactics.Utilities.Common.ExtensionMehods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace _6tactics.Cms.Web.App_Logic
{
    public static class ElementsVisibilityUtility
    {
        public static string GetFilePathById(string uploadsPath, int id)
        {
            string path = Path.Combine(uploadsPath, id.ToString());
            return Directory.EnumerateFiles(path).FirstOrDefault();
        }

        public static string CreateImagePath(int? id, string fileName)
        {
            if (id == null || fileName == null) return null;

            return $"/Content/uploads/{id}/{fileName}";
        }

        public static bool IsTitleOfContent(IContentItem currentContentItem)
        {
            if (currentContentItem.ContentType == ContentType.Project) return true;

            if (currentContentItem.Parent == null) return false;

            if ((currentContentItem.Parent.ContentType == ContentType.ContentElement || currentContentItem.Parent.ContentType == ContentType.FileElement)
                && currentContentItem.Parent.ContentType == currentContentItem.ContentType)
                return false;

            return true;
        }

        public static bool IsElementWidthPanelVisible(IContentItem currentContentItem)
        {
            if (currentContentItem.ContentType == ContentType.Project) return true;

            if (currentContentItem.ContentType == ContentType.Page || currentContentItem.ContentType == ContentType.Footer) return true;

            if (currentContentItem.Parent == null) return false;

            if ((currentContentItem.Parent.ContentType == ContentType.ContentElement || currentContentItem.Parent.ContentType == ContentType.FileElement)
                && currentContentItem.Parent.ContentType == currentContentItem.ContentType)
                return false;

            return true;
        }

        public static bool IsSectionTitleVsibilityVisible(AdminActionsViewModel adminActionsViewModel)
        {
            return adminActionsViewModel.CreateEditDataViewModel.SelectedContentType != ContentType.Project &&
                   !adminActionsViewModel.CreateEditDataViewModel.IsParentElementOrFile;
        }

        public static bool IsCollapsingButtonVisible(IContentItem currentContentItem)
        {
            return
                (currentContentItem.ContentType != ContentType.Project &&
                currentContentItem.ContentType != ContentType.Language &&
                currentContentItem.ContentType != ContentType.Page &&
                currentContentItem.ContentType != ContentType.Footer &&
                currentContentItem.ContentType != ContentType.ContentElement &&
                currentContentItem.ContentType != ContentType.FileElement) ||
                currentContentItem.ContentItems.Count >= 1;
        }

        //public static bool IsElementVisible(ElementVisibility? elementVisibility, bool isUserAuthenticated)
        //{
        //    if (elementVisibility == ElementVisibility.Visible || elementVisibility == ElementVisibility.VisibleForAdminOnly && isUserAuthenticated)
        //        return true;
        //    if (elementVisibility == ElementVisibility.Hidden)
        //        return false;

        //    return false;
        //}

        public static bool IsElementVisible(ElementVisibility? elementVisibility, bool isAllowedByUserOrGroup)
        {
            return elementVisibility == ElementVisibility.Visible ||
                (elementVisibility == ElementVisibility.VisibleForAdminOnly && isAllowedByUserOrGroup);
        }

        public static bool IsRouteTitleElementVisibleOnCreatePage(ContentType selectedContentType)
        {
            return selectedContentType == ContentType.Language || selectedContentType == ContentType.Page;
        }

        public static bool IsRouteTitleElementVisibleOnEditPage(ContentType selectedContentType)
        {
            return selectedContentType == ContentType.Page;
        }

        // Deprecated!!
        public static bool IsSectionVisible(ElementVisibility? elementVisibility, bool isUserAuthenticated,
            List<ContentItemViewModel> contentItems)
        {
            bool isAnyOfContentItemsVisible = contentItems != null && contentItems.Count > 0
                && contentItems.Any(contentItem => contentItem.ElementVisibility == ElementVisibility.Visible);

            bool isSesctionVisible = IsElementVisible(elementVisibility, isUserAuthenticated);

            return isSesctionVisible && isAnyOfContentItemsVisible;
        }

        public static bool IsSectionVisible(ElementVisibility? elementVisibility, bool isUserAuthenticated, SectionViewModel currentItem)
        {
            bool isContentItemEmptyRow = currentItem.DisplayType == "EmptyRow";
            bool isSesctionVisible = IsElementVisible(elementVisibility, isUserAuthenticated);

            bool isAnyOfContentItemsVisible = currentItem.Items != null && currentItem.Items.Any()
                && currentItem.Items.Any(contentItem => contentItem.ElementVisibility == ElementVisibility.Visible);

            return isSesctionVisible && (isContentItemEmptyRow || isAnyOfContentItemsVisible);
        }

        public static bool IsPageSelected(IContentItem currentContentItem, string selectedRouteTitle)
        {
            if (string.IsNullOrEmpty(selectedRouteTitle)) return false;

            return currentContentItem.RouteTitle == selectedRouteTitle ||
                currentContentItem.ContentItems.Hierarchical(i => i.ContentItems).FirstOrDefault(i => i.Item.RouteTitle == selectedRouteTitle) != null;
        }

        public static bool ShowContentItemIf(ContentItemViewModel contentItem, ContentType contentType, string displayType, bool isUserAuthenticated)
        {
            return contentItem.ContentType == contentType && contentItem.DisplayType == displayType &&
                   IsElementVisible(contentItem.ElementVisibility, isUserAuthenticated);
        }

        public static bool IsSectionTitleVisible(SectionTitleVisibility? sectionTitleVisibility)
        {
            return sectionTitleVisibility == SectionTitleVisibility.Visible;
        }

        public static string GetCurrentArea(HttpRequestBase request)
        {
            return request.RequestContext.RouteData.DataTokens.ContainsKey("area")
                ? request.RequestContext.RouteData.DataTokens["area"].ToString() : "";
        }

        public static string CurrentController => HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();

        public static bool IsCurrentControllerAdmin => HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()
            .Equals("admin", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsCurrentControllerSetup => HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()
            .Equals("setup", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsCurrentControllerPluginsManager => HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()
            .Equals("pluginsmanager", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsCurrentControllerFileManager => HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()
            .Equals("filemanager", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsCurrentControllerManage => HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()
         .Equals("manage", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsCurrentControllerUserAdministration => HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()
         .Equals("useradministration", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsCurrentControllerRolesAdministration => HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()
         .Equals("rolesadministration", StringComparison.InvariantCultureIgnoreCase);

        public static string CurrentAction => HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();

        public static string LanguageFromAdminTreeView { get; set; }

        public static string CurrentLanguage
        {
            get
            {
                var currentLanguage = HttpContext.Current.Request.RequestContext.RouteData.Values["lang"];
                return currentLanguage?.ToString();
            }
        }

        public static string CurrentPageId
        {
            get
            {
                var currentPageId = HttpContext.Current.Request.RequestContext.RouteData.Values["pageId"];
                return currentPageId?.ToString();
            }
        }

        public static string CurrentRouteTitle
        {
            get
            {
                var currentRouteTitle = HttpContext.Current.Request.RequestContext.RouteData.Values["routeTitle"];
                return currentRouteTitle?.ToString();
            }
        }

        public static string CurrentId => HttpContext.Current.Request.RequestContext.RouteData.Values["id"].ToString();

        public static string CurrentParentId => HttpContext.Current.Request.RequestContext.RouteData.Values["parentId"].ToString();

        public static bool IsHomePage
        {
            get
            {
                var currentControllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                return currentControllerName.Equals("index", StringComparison.InvariantCultureIgnoreCase) || currentControllerName == "_TreeViewHome";
            }

        }

        public static bool IsPrioritiePage
        {
            get
            {
                var currentControllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                return currentControllerName.Equals("priorities", StringComparison.InvariantCultureIgnoreCase) || currentControllerName == "_TreeViewPriorities";
            }
        }

        public static bool IsCreatePage => HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString()
            .Equals("create", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsEditPage => HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString()
            .Equals("edit", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsSetupPage => HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString()
            .Equals("setup", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsPluginsPage => HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString()
            .Equals("plugins", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsDetailsPage => HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString()
            .Equals("details", StringComparison.InvariantCultureIgnoreCase);
        public static bool IsDeletePage => HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString()
            .Equals("delete", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsManagePage => HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString()
            .Equals("manage", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsLoginPage => HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString()
            .Equals("login", StringComparison.InvariantCultureIgnoreCase);
    }
}
