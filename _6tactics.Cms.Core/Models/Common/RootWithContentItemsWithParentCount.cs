using _6tactics.Cms.Core.Entities;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.Models.Common
{
    public class RootWithContentItemsWithParentCount
    {
        public ContentItem ContentItem { get; set; }
        public List<ContentItemWithParentCount> ContentItemsWithParentCount { get; set; }
    }
}
