using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _DataAccess.Repositories
{
    public interface IContentItemRepository
    {
        IQueryable<ContentItem> AllContentItems { get; }
        ContentItem ProjectItem { get; }
        ContentItem ProjectItemWithChildrens { get; }
        IQueryable<string> LanguageList { get; }
        IEnumerable<ContentItem> GetContentItemsByLanguage(string language);
        ContentItem GetContentItemById(int? id);
        IQueryable<ContentItem> GetChildrensOfContentItemById(int? id);
        bool IsChildElementWithSameNameExist(int? parentId, int? currentContentItemId, string name);
        bool IsFooterUnderSameLanguageExist(int? id, string language);
        bool IsPageWithSameNameUnderSelectedLanguageExist(int? id, string language, string routeTitle);
        ContentItem GetParentContentItemById(int? parentId);
        IQueryable<ContentItem> GetParentContentItemsById(int? parentId);
        ContentItem GetLanguageItemByTitle(string languageTitle);
        ContentItem GetPageItemFromSelectedLanguageById(string language, int? id);
        ContentItem GetFilteredResults(string language, int? pageId);
        ContentItem GetPageItemById(int? pageId);
        ContentItem GetSelectedPageByRouteTitle(ContentItem selectedLanguage, string selectedPageRouteTitle, Func<ElementVisibility?, bool, bool> isElementVisible);
        IEnumerable<ContentItem> GetPagesBySelectedLanguage(string language);
        IEnumerable<ContentItem> GetContentItemHierarchically(int? parentId);
        void InsertContentItem(ContentItem contentItem);
        void UpdateContentItem(int? id, ContentItem contentItem);
        void UpdateContentItem(ContentItem contentItem);
        void DeleteContentItem(ContentItem contentItem);
        void DeleteContentItemByIdWithChildrens(int? id);
        int ContentItemsCount { get; }
        int LanguagesCount { get; }
        int PagesCount { get; }
        int FootersCount { get; }
        int ContentElementsCount { get; }
        int FileElementsCount { get; }
    }
}