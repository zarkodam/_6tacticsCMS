using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Models.Common;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.Entities
{

    public class ContentItem : IContentItem
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string RouteTitle { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Culture { get; set; }
        public int Priority { get; set; }
        public LinkOption? LinkOption { get; set; }
        public string LinkTo { get; set; }
        public SectionTitleVisibility? SectionTitleVisibility { get; set; }
        public ElementVisibility? ElementVisibility { get; set; }
        public ContentType ContentType { get; set; }
        public int? ParentId { get; set; }
        public ContentItem Parent { get; set; }
        public string DisplayType { get; set; }
        public ICollection<ContentItem> ContentItems { get; set; }
        public string FileUrl { get; set; }
        public string ElementWidth { get; set; }
        public ContentItem()
        {
            ContentItems = new List<ContentItem>();
        }
    }
}