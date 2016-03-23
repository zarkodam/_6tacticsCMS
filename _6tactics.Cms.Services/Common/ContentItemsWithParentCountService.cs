using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Models.Common;
using _DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _6tactics.Cms.Services.Common
{
    public class ContentItemsWithParentCountService : IContentItemsWithParentCountService
    {
        #region Fields

        private readonly IContentItemRepository _contentItemRepository;
        private Func<ContentItem, bool> _childrensFilter;
        private readonly List<ContentItemWithParentCount> _contentItemsWithParentCount = new List<ContentItemWithParentCount>();
        private readonly Dictionary<ContentItem, List<ContentItemWithParentCount>> _rootWithContentItemsWithParentCount = new Dictionary<ContentItem, List<ContentItemWithParentCount>>();

        #endregion


        #region Ctor


        public ContentItemsWithParentCountService(IContentItemRepository contentItemRepository)
        {
            _contentItemRepository = contentItemRepository;
        }

        #endregion


        #region Actions

        public Dictionary<ContentItem, List<ContentItemWithParentCount>> GetFilteredByRootAndChildAndGroupedByRoot(Func<ContentItem, bool> rootFilter, Func<ContentItem, bool> childrensFilter, IEnumerable<ContentItem> customizedDataToFilter = null)
        {
            _childrensFilter = childrensFilter;
            IEnumerable<ContentItem> modelToFilter = customizedDataToFilter ?? _contentItemRepository.ProjectItemWithChildrens.ContentItems;
            AddRootItemsToList(modelToFilter.Where(rootFilter), true);

            return _rootWithContentItemsWithParentCount;
        }

        public List<ContentItemWithParentCount> GetFilteredByRootAndChild(Func<ContentItem, bool> rootFilter, Func<ContentItem, bool> childrensFilter, IEnumerable<ContentItem> customizedDataToFilter = null)
        {
            _childrensFilter = childrensFilter;
            IEnumerable<ContentItem> modelToFilter = customizedDataToFilter ?? _contentItemRepository.ProjectItemWithChildrens.ContentItems;
            AddRootItemsToList(modelToFilter.Where(rootFilter), false);

            return _contentItemsWithParentCount;
        }

        public List<ContentItemWithParentCount> GetFiltered(Func<ContentItem, bool> filter, IEnumerable<ContentItem> customizedDataToFilter = null)
        {
            _childrensFilter = filter;
            IEnumerable<ContentItem> modelToFilter = customizedDataToFilter ?? _contentItemRepository.ProjectItemWithChildrens.ContentItems;
            AddRootItemsToList(modelToFilter.Where(filter), false);

            return _contentItemsWithParentCount;
        }

        #endregion


        #region Helpers


        private int _counter;
        private int _counterGroup = 1;
        private void AddChildrenItemsToList(IEnumerable<ContentItem> childrenContentItems, ContentItem root, bool groupByRoot)
        {
            _counter += 1;

            foreach (var contentItem in childrenContentItems.Where(_childrensFilter))
            {
                _contentItemsWithParentCount.Add(new ContentItemWithParentCount { Depth = _counter, ContentItem = contentItem });

                if (groupByRoot)
                {
                    if (_rootWithContentItemsWithParentCount.ContainsKey(root))
                        _rootWithContentItemsWithParentCount[root] = _contentItemsWithParentCount;
                    else
                        _rootWithContentItemsWithParentCount.Add(root, _contentItemsWithParentCount);
                }

                if (contentItem.ContentItems == null) continue;

                AddChildrenItemsToList(contentItem.ContentItems, root, groupByRoot);
            }

            _counterGroup = _counter - 1;
            _counter = _counterGroup;
        }

        private void AddRootItemsToList(IEnumerable<ContentItem> parentContentItems, bool groupByRoot)
        {
            foreach (var contentItem in parentContentItems)
            {
                if (!groupByRoot)
                    _contentItemsWithParentCount.Add(new ContentItemWithParentCount { Depth = 0, ContentItem = contentItem });

                if (contentItem.ContentItems == null) continue;

                AddChildrenItemsToList(contentItem.ContentItems, contentItem, groupByRoot);
            }
        }

        #endregion
    }
}
