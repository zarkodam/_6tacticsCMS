using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.ViewModels.Admin;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _6tactics.Cms.Services.Admin
{
    public interface IAdminLogicService
    {
        ContentItem MapAndSaveAndReturnContentItem(AdminActionsViewModel adminCreateViewModel);
        ContentItem GetFilteredResults(string language, int? pageId);
        int? GetRedirectionId(AdminActionsViewModel adminActionsViewModel, ContentItem contentItem);
        bool IsChildElemetnWithSameNameExist(AdminActionsViewModel adminCreateViewModel);
        bool IsFooterUnderSameLanguageExist(AdminActionsViewModel adminCreateViewModel);
        bool IsPageWithSameNameUnderSelectedLanguageExist(AdminActionsViewModel adminCreateViewModel);
        void UpdateContentItemAndHisChildren(AdminActionsViewModel adminActionsViewModel);
        void SetPriorities(FormCollection formItem);
        IEnumerable<ContentItem> GetDataForDelete(int? id);
        void DeleteContentItems(int? id);
    }
}