using _6tactics.Cms.Core.Entities;

namespace _6tactics.Cms.Core.Models.Common
{
    public class ContentItemWithParentCount
    {
        public int Depth { get; set; }
        public ContentItem ContentItem { get; set; }
    }
}
