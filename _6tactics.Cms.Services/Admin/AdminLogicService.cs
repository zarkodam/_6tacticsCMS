using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Utilities;
using _6tactics.Cms.Core.ViewModels.Admin;
using _6tactics.Cms.Core.ViewModels.Common;
using _6tactics.Utilities.ObjectMapping;
using _DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _6tactics.Cms.Services.Admin
{
    public class AdminLogicService : IAdminLogicService
    {
        #region Fields

        private readonly IUnitOfWork _uof;

        #endregion


        #region Constructors

        public AdminLogicService(IUnitOfWork unitOfWork)
        {
            _uof = unitOfWork;
        }

        #endregion


        #region Methods

        // Filtered results for Index and Priorities view
        public ContentItem GetFilteredResults(string language, int? pageId)
        {
            bool isLanguageSelected = !string.IsNullOrWhiteSpace(language);

            if (isLanguageSelected && pageId != null)
                return _uof.ContentItemRepo.GetPageItemById(pageId);

            if (isLanguageSelected)
                return _uof.ContentItemRepo.GetLanguageItemByTitle(language);

            return _uof.ContentItemRepo.ProjectItem;
        }

        // Map and save and return content item
        public ContentItem MapAndSaveAndReturnContentItem(AdminActionsViewModel adminCreateViewModel)
        {
            SetPriorityForFooter(adminCreateViewModel);

            var contentItem = ObjectMapper.Map<ContentItemViewModel, ContentItem>(adminCreateViewModel.ContentItemViewModel);

            _uof.ContentItemRepo.InsertContentItem(contentItem);
            _uof.Save();

            return contentItem;
        }

        public int? GetRedirectionId(AdminActionsViewModel adminActionsViewModel, ContentItem contentItem)
        {
            var contentItemByParentId = _uof.ContentItemRepo.GetContentItemById(contentItem.ParentId);

            if (contentItemByParentId == null) return null;

            if (adminActionsViewModel.CreateEditDataViewModel != null && adminActionsViewModel.CreateEditDataViewModel.IsCreateChild == true)
            {
                return
                    contentItemByParentId.ContentType == ContentType.Project ||
                    contentItemByParentId.ContentType == ContentType.Language ||
                    contentItemByParentId.ContentType == ContentType.Page ||
                    contentItemByParentId.ContentType == ContentType.Footer

                    ? contentItem.Id : contentItem.ParentId;
            }

            return null;
        }

        // Checks ContentType.Language, ContentType.ContentElement and ContentType.File
        public bool IsChildElemetnWithSameNameExist(AdminActionsViewModel adminCreateViewModel)
        {
            return _uof.ContentItemRepo.IsChildElementWithSameNameExist(
                adminCreateViewModel.ContentItemViewModel.ParentId,
                adminCreateViewModel.ContentItemViewModel.Id,
                adminCreateViewModel.ContentItemViewModel.Title);
        }

        // Checks ContentType.Footer
        public bool IsFooterUnderSameLanguageExist(AdminActionsViewModel adminCreateViewModel)
        {
            return adminCreateViewModel.ContentItemViewModel.ContentType == ContentType.Footer &&
                _uof.ContentItemRepo.IsFooterUnderSameLanguageExist(
                adminCreateViewModel.ContentItemViewModel.Id,
                adminCreateViewModel.CreateEditDataViewModel.ElementLanguage);
        }

        // Checks ContentType.Page
        public bool IsPageWithSameNameUnderSelectedLanguageExist(AdminActionsViewModel adminCreateViewModel)
        {
            using (IUnitOfWork uof = new UnitOfWork())
                return uof.ContentItemRepo.IsPageWithSameNameUnderSelectedLanguageExist(
                    adminCreateViewModel.ContentItemViewModel.Id,
                    adminCreateViewModel.CreateEditDataViewModel.ElementLanguage,
                    adminCreateViewModel.ContentItemViewModel.RouteTitle);
        }

        public void UpdateContentItemAndHisChildren(AdminActionsViewModel adminActionsViewModel)
        {
            SetPriorityForFooter(adminActionsViewModel);

            // Mapping viewmodel data to model data
            var contentItem = ObjectMapper.Map<ContentItemViewModel, ContentItem>(adminActionsViewModel.ContentItemViewModel);

            // Updateing and saveing
            _uof.ContentItemRepo.UpdateContentItem(contentItem);

            // Get all children items by current content item id
            List<ContentItem> childrenItemByCurrentContentItemId = _uof.ContentItemRepo.GetParentContentItemsById(contentItem.Id).ToList();

            // Update all children content items if ContetType or DisplayType changed
            var currentContentType = adminActionsViewModel.ContentItemViewModel.ContentType;
            if (currentContentType != ContentType.Project && currentContentType != ContentType.Language &&
                currentContentType != ContentType.Page && currentContentType != ContentType.Footer)
                foreach (var childrenItem in childrenItemByCurrentContentItemId)
                {
                    if (childrenItem != null && childrenItem.ContentType != contentItem.ContentType)
                        childrenItem.ContentType = contentItem.ContentType;
                    if (childrenItem != null && childrenItem.DisplayType != contentItem.DisplayType)
                        childrenItem.DisplayType = contentItem.DisplayType;

                    if (childrenItem != null && (childrenItem.ContentType != contentItem.ContentType || childrenItem.DisplayType != contentItem.DisplayType))
                        _uof.ContentItemRepo.UpdateContentItem(childrenItem);
                }

            _uof.Save();
        }

        public IEnumerable<ContentItem> GetDataForDelete(int? id)
        {
            var contentItemsWithChildrens = _uof.ContentItemRepo.GetContentItemHierarchically(id);
            return contentItemsWithChildrens;
        }

        public void SetPriorities(FormCollection formItem)
        {
            string[] ids = formItem.GetValues("contentItem.Id");
            string[] priorities = formItem.GetValues("contentItem.Priority");
            string[] contentTypes = formItem.GetValues("contentItem.ContentType");

            if (ids != null && priorities != null && contentTypes != null)
            {
                for (var i = 0; i < ids.Length; i++)
                {
                    if (contentTypes[i] == ContentType.Footer.ToString()) continue;

                    ContentItem contentItem = _uof.ContentItemRepo.GetContentItemById(Convert.ToInt32(ids[i]));
                    contentItem.Priority = priorities[i] != null ? Convert.ToInt32(priorities[i]) : 0;

                    _uof.ContentItemRepo.UpdateContentItem(contentItem);
                }
            }

            _uof.Save();
        }

        // Cascade delete with uploaded files removing
        public void DeleteContentItems(int? id)
        {
            foreach (var contentItem in _uof.ContentItemRepo.GetContentItemHierarchically(id))
            {
                if (contentItem.ContentType == ContentType.Project)
                    SeoUtility.DeleteSeoData();
                _uof.ContentItemRepo.DeleteContentItem(contentItem);
            }
            _uof.Save();
        }

        #endregion


        #region Helpers

        private void SetPriorityForFooter(AdminActionsViewModel adminActionsViewModel)
        {
            if (adminActionsViewModel.ContentItemViewModel.ContentType != ContentType.Footer) return;

            adminActionsViewModel.ContentItemViewModel.Priority = 10000;
            adminActionsViewModel.ContentItemViewModel.DisplayType = null;
            adminActionsViewModel.ContentItemViewModel.ElementWidth = null;
        }
        #endregion

    }
}
