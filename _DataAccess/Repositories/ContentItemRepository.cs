using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Utilities.Common.ExtensionMehods;
using _DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace _DataAccess.Repositories
{
    public class ContentItemRepository : IContentItemRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<ContentItem> _dbSet;
        private IList<ContentItem> _dataForDelete = new List<ContentItem>();

        public ContentItemRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<ContentItem>();
        }

        public IQueryable<ContentItem> AllContentItems => _dbSet;

        public ContentItem ProjectItem
        {
            get { return _dbSet.FirstOrDefault(i => i.ContentType == ContentType.Project); }
        }

        public ContentItem ProjectItemWithChildrens
        {
            get { return _dbSet.ToList().FirstOrDefault(i => i.ContentType == ContentType.Project); }
        }

        public IQueryable<string> LanguageList
        {
            get { return AllContentItems.Where(i => i.ContentType == ContentType.Language).Select(i => i.Title); }
        }

        public IEnumerable<ContentItem> GetContentItemsByLanguage(string language)
        {
            return ProjectItemWithChildrens.ContentItems
                .Where(i => i.ContentType == ContentType.Language && i.Title.Equals(language, StringComparison.InvariantCultureIgnoreCase));
        }

        public ContentItem GetContentItemById(int? id)
        {
            return _dbSet.Add(_dbSet.FirstOrDefault(i => i.Id == id));
        }

        public IQueryable<ContentItem> GetChildrensOfContentItemById(int? id)
        {
            return _dbSet.Where(i => i.ParentId == id);
        }

        public bool IsChildElementWithSameNameExist(int? parentId, int? currentContentItemId, string name)
        {
            return GetChildrensOfContentItemById(parentId)
                    .Where(i => i.ContentType == ContentType.Language || i.ContentType == ContentType.ContentElement || i.ContentType == ContentType.FileElement)
                    .Any(i => i.Id != currentContentItemId && i.Title.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool IsFooterUnderSameLanguageExist(int? id, string language)
        {
            var languagesElements = AllContentItems.Where(i => i.ContentType == ContentType.Language && i.Title == language);
            var footerElement = languagesElements.Select(i => i.ContentItems.FirstOrDefault(j => j.ContentType == ContentType.Footer));

            return id == null
                ? footerElement.Any(i => i != null)
                : footerElement.Any(i => i.Id != id);
        }

        public bool IsPageWithSameNameUnderSelectedLanguageExist(int? id, string language, string routeTitle)
        {
            var pageElements = GetContentItemsByLanguage(language).Hierarchical(i => i.ContentItems.Where(j => j.ContentType == ContentType.Page));

            return id == null
                ? pageElements.Any(i => !string.IsNullOrEmpty(routeTitle) && i.Item.RouteTitle == routeTitle)
                : pageElements.Any(i => i.Item.Id != id && !string.IsNullOrEmpty(routeTitle) && i.Item.RouteTitle == routeTitle);
        }


        public ContentItem GetParentContentItemById(int? parentId)
        {
            return _dbSet.FirstOrDefault(i => i.ParentId == parentId);
        }

        public IQueryable<ContentItem> GetParentContentItemsById(int? parentId)
        {
            return _dbSet.Where(i => i.ParentId == parentId);
        }

        public ContentItem GetLanguageItemByTitle(string languageTitle)
        {
            return _dbSet.ToList().FirstOrDefault(i => i.ContentType == ContentType.Language && i.Title == languageTitle);
        }

        public ContentItem GetPageItemFromSelectedLanguageById(string language, int? id)
        {
            return GetLanguageItemByTitle(language).ContentItems.FirstOrDefault(i => i.ContentType == ContentType.Page && i.Id == id);
        }

        public ContentItem GetFilteredResults(string language, int? pageId)
        {
            bool isLanguageSelected = !string.IsNullOrWhiteSpace(language);

            if (isLanguageSelected && pageId != null)
                return GetPageItemById(pageId);

            if (isLanguageSelected)
                return GetLanguageItemByTitle(language);

            return ProjectItemWithChildrens;
        }

        public ContentItem GetPageItemById(int? pageId)
        {
            return _dbSet.ToList().FirstOrDefault(i => i.ContentType == ContentType.Page && i.Id == pageId);
        }

        private ContentItem _selectedPage;
        private void GetSelectedPage(ContentItem selectedLanguage, string selectedPageRouteTitle, Func<ElementVisibility?, bool, bool> isElementVisible)
        {
            foreach (ContentItem page in selectedLanguage.ContentItems.Where(i => i.ContentType == ContentType.Page && isElementVisible(i.ElementVisibility, true)).OrderBy(i => i.Priority))
            {
                if (page.RouteTitle == selectedPageRouteTitle)
                    _selectedPage = page;

                GetSelectedPage(page, selectedPageRouteTitle, isElementVisible);
            }
        }

        public ContentItem GetSelectedPageByRouteTitle(ContentItem selectedLanguage, string selectedPageRouteTitle, Func<ElementVisibility?, bool, bool> isElementVisible)
        {
            //GetSelectedPage(selectedLanguage, selectedPageRouteTitle, (visibility, isAllowed) => visibility == elementVisibility && isAllowed == isUserGroupAllowed);
            GetSelectedPage(selectedLanguage, selectedPageRouteTitle, isElementVisible);
            return _selectedPage;
        }

        public IEnumerable<ContentItem> GetPagesBySelectedLanguage(string language)
        {
            //throw new System.NotImplementedException();
            return ProjectItem.ContentItems.Where(i => i.ContentType == ContentType.Language && i.Title.Equals("language", StringComparison.InvariantCultureIgnoreCase));
        }

        private void ContentItemHierarchically(int? parentId)
        {
            List<ContentItem> childItems = GetParentContentItemsById(parentId).ToList();

            foreach (var item in childItems)
            {
                _dataForDelete.Add(item);
                ContentItemHierarchically(item.Id);
            }
        }

        public void UpdateContentItem(int? id, ContentItem contentItem)
        {
            _dbSet.Attach(contentItem);
            _context.Entry(contentItem).State = EntityState.Modified;
        }

        public void UpdateContentItem(ContentItem contentItem)
        {
            _dbSet.Attach(contentItem);
            _context.Entry(contentItem).State = EntityState.Modified;
        }

        public IEnumerable<ContentItem> GetContentItemHierarchically(int? parentId)
        {
            _dataForDelete = new List<ContentItem> { GetContentItemById(parentId) };
            ContentItemHierarchically(parentId);
            return _dataForDelete;
        }

        public void InsertContentItem(ContentItem contentItem)
        {
            _dbSet.Add(contentItem);
            _context.Entry(contentItem).State = EntityState.Added;
        }

        public void DeleteContentItem(ContentItem contentItem)
        {
            _dbSet.Remove(contentItem);
            _context.Entry(contentItem).State = EntityState.Deleted;
        }

        public void DeleteContentItemByIdWithChildrens(int? id)
        {
            foreach (var contentItem in GetContentItemHierarchically(id))
                DeleteContentItem(contentItem);
        }

        public int ContentItemsCount => _dbSet.Count();

        public int LanguagesCount
        {
            get { return _dbSet.Count(i => i.ContentType == ContentType.Language); }
        }

        public int PagesCount
        {
            get { return _dbSet.Count(i => i.ContentType == ContentType.Page); }
        }

        public int FootersCount
        {
            get { return _dbSet.Count(i => i.ContentType == ContentType.Footer); }
        }

        public int ContentElementsCount
        {
            get { return _dbSet.Count(i => i.ContentType == ContentType.ContentElement); }
        }

        public int FileElementsCount
        {
            get { return _dbSet.Count(i => i.ContentType == ContentType.FileElement); }
        }

    }
}
