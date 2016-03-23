using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Models.Common;
using System;
using System.Collections.Generic;

namespace _6tactics.Cms.Services.Common
{
    public interface IContentItemsWithParentCountService
    {
        Dictionary<ContentItem, List<ContentItemWithParentCount>> GetFilteredByRootAndChildAndGroupedByRoot(Func<ContentItem, bool> rootFilter, Func<ContentItem, bool> childrensFilter, IEnumerable<ContentItem> customizedDataToFilter = null);
        List<ContentItemWithParentCount> GetFilteredByRootAndChild(Func<ContentItem, bool> rootFilter, Func<ContentItem, bool> childrensFilter, IEnumerable<ContentItem> customizedDataToFilter = null);
        List<ContentItemWithParentCount> GetFiltered(Func<ContentItem, bool> filter, IEnumerable<ContentItem> customizedDataToFilter = null);
    }
}