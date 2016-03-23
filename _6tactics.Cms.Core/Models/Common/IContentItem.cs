using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.Models.Common
{
    public interface IContentItem
    {
        int? Id { get; set; }
        string Title { get; set; }
        string RouteTitle { get; set; }
        string Summary { get; set; }
        string Content { get; set; }
        string Culture { get; set; }
        int Priority { get; set; }
        LinkOption? LinkOption { get; set; }
        string LinkTo { get; set; }
        SectionTitleVisibility? SectionTitleVisibility { get; set; }
        ElementVisibility? ElementVisibility { get; set; }
        ContentType ContentType { get; set; }
        int? ParentId { get; set; }
        ContentItem Parent { get; set; }
        string DisplayType { get; set; }
        ICollection<ContentItem> ContentItems { get; set; }
        string FileUrl { get; set; }
        string ElementWidth { get; set; }
    }
}