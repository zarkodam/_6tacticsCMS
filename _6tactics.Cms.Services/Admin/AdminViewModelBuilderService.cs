using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Helpers;
using _6tactics.Cms.Core.ViewModels.Admin;
using _6tactics.Cms.Core.ViewModels.Common;
using _6tactics.Utilities.FileSystem;
using _6tactics.Utilities.ObjectMapping;
using _DataAccess.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _6tactics.Cms.Services.Admin
{
    public class AdminViewModelBuilderService : IAdminViewModelBuilderService
    {
        #region Fields

        private readonly IUnitOfWork _uof;
        private readonly List<SelectListItem> _pageList = new List<SelectListItem>();

        #endregion


        #region Constructors


        public AdminViewModelBuilderService(IUnitOfWork unitOfWork)
        {
            _uof = unitOfWork;
        }

        #endregion


        #region Helpers

        private void PageListBuilder(ContentItem contentItemToCheck, string language = null, string selectedElement = null)
        {
            foreach (ContentItem contentItem in contentItemToCheck.ContentItems.OrderBy(i => i.Priority))
            {
                if (contentItem.ContentType == ContentType.Language)
                    PageListBuilder(contentItem, contentItem.Title, selectedElement);

                if (!string.IsNullOrEmpty(language) && contentItem.ContentType == ContentType.Page)
                {
                    string listItemText = string.Concat("/", language.ToLower(), "/", contentItem.RouteTitle);
                    string listItemValue = string.Concat("/web/index", listItemText);
                    _pageList.Add(new SelectListItem { Value = listItemValue, Text = listItemText, Selected = listItemValue.Equals(selectedElement) });

                    if (contentItem.ContentItems.Any(i => i.ContentType == ContentType.Page))
                        PageListBuilder(contentItem, language, selectedElement);
                }

            }
        }

        private IEnumerable<SelectListItem> CreatePageList(ContentItem contentItemToCheck, string selectedElement = null)
        {
            PageListBuilder(contentItemToCheck, null, selectedElement);
            return _pageList;
        }

        #endregion


        // For Create, Edit and Details
        public AdminActionsViewModel CreateAdminActionsViewModel(
                int? id,
                ContentItemAction contentItemAction,
                string language = null,
                StatisticsViewModel statisticsViewModel = null)
        {
            ContentItem project = _uof.ContentItemRepo.ProjectItemWithChildrens;

            ContentItem contentItemByParentId = _uof.ContentItemRepo.GetContentItemById(id);

            if (contentItemByParentId == null) return null;

            ContentType slectedElementContentType = contentItemByParentId.ContentType;
            string slectedElementDisplayType = contentItemByParentId.DisplayType;

            bool isParentElementOrFile;

            CreateEditDataViewModel createEditDataViewModel = null;
            ContentItemViewModel contentItemViewModel = null;

            if (contentItemAction == ContentItemAction.Create || contentItemAction == ContentItemAction.AlreadyExistInDbOnCreate ||
                contentItemAction == ContentItemAction.FooterUnderLanguageAlreadyExistOnCreate)
            {
                isParentElementOrFile = slectedElementContentType == ContentType.ContentElement || slectedElementContentType == ContentType.FileElement;

                createEditDataViewModel = new CreateEditDataViewModel
                {
                    ParentId = id,
                    ElementLanguage = language,
                    SelectedTitle = contentItemByParentId.Title,
                    SelectedContentType = slectedElementContentType,
                    SelectedDisplayType = slectedElementDisplayType,
                    IsParentElementOrFile = isParentElementOrFile,
                    DisplayTypeList = DropDownHelper.GetDisplayTypeListForCreatePage(slectedElementContentType),
                    ElementWidthList = DropDownHelper.GetElementWidthList,
                    PageList = CreatePageList(project),
                    IsCreateChild = true
                };

                contentItemViewModel = new ContentItemViewModel
                {
                    ContentType = slectedElementContentType,
                    DisplayType = slectedElementDisplayType
                };
            }
            else if (contentItemAction == ContentItemAction.Edit || contentItemAction == ContentItemAction.AlreadyExistInDbOnEdit ||
                contentItemAction == ContentItemAction.Details || contentItemAction == ContentItemAction.FooterUnderLanguageAlreadyExistOnEdit)
            {
                int? properIdForContentItem = contentItemByParentId.ContentType == ContentType.Project
                    ? contentItemByParentId.Id
                    : contentItemByParentId.ParentId;

                ContentItem parentContentItem = _uof.ContentItemRepo.GetContentItemById(properIdForContentItem);

                var parentContentType = parentContentItem?.ContentType ?? ContentType.Project;
                isParentElementOrFile = parentContentItem != null && (parentContentType == ContentType.ContentElement || parentContentType == ContentType.FileElement);

                createEditDataViewModel = new CreateEditDataViewModel
                {
                    ElementLanguage = language,
                    SelectedTitle = contentItemByParentId.Title,
                    CurrentContentType = slectedElementContentType,
                    ParentContentType = parentContentType,
                    IsParentElementOrFile = isParentElementOrFile,
                    DisplayTypeList = DropDownHelper.GetDisplayTypeListForEditPage(slectedElementContentType),
                    ElementWidthList = DropDownHelper.GetElementWidthList,
                    PageList = CreatePageList(project, contentItemByParentId.LinkTo),
                    IsCreateChild = true
                };
                contentItemViewModel = ObjectMapper.Map<ContentItem, ContentItemViewModel>(contentItemByParentId);
            }
            else if (contentItemAction == ContentItemAction.Index || contentItemAction == ContentItemAction.Priorities)
            {

            }

            return new AdminActionsViewModel
            {
                CreateEditDataViewModel = createEditDataViewModel,
                ContentItemViewModel = contentItemViewModel,
                ContentItemModel = project,
                StatisticsViewModel = statisticsViewModel
            };
        }

        private StatisticsViewModel CreateStatisticsViewModel(ContentItem contentItem, string uploadFolderPath)
        {
            return new StatisticsViewModel
            {
                ContentItemsCount = _uof.ContentItemRepo.ContentItemsCount,
                LanguagesCount = _uof.ContentItemRepo.LanguagesCount,
                PagesCount = _uof.ContentItemRepo.PagesCount,
                FootersCount = _uof.ContentItemRepo.FootersCount,
                ContentElementsCount = _uof.ContentItemRepo.ContentElementsCount,
                FileElementsCount = _uof.ContentItemRepo.FileElementsCount,
                FileElementsSizeCount = new FileSystemReader(uploadFolderPath).GetDirectories(addFiles: true).GetSizeInMegabytes()
            };
        }

        // For Index and Priorities
        public AdminActionsViewModel CreateAdminActionsViewModel(string language, int? pageId, string uploadFolderPath)
        {
            var filteredContentItem = _uof.ContentItemRepo.GetFilteredResults(language, pageId);

            return new AdminActionsViewModel
            {
                ContentItemModel = filteredContentItem,
                StatisticsViewModel = CreateStatisticsViewModel(filteredContentItem, uploadFolderPath)
            };
        }

    }
}
