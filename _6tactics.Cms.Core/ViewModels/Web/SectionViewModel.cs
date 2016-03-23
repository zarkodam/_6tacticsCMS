using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.ViewModels.Common;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.ViewModels.Web
{
    public class SectionViewModel
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public ContentType ContentType { get; set; }

        public string DisplayType { get; set; }

        public string FileUrl { get; set; }

        public SectionTitleVisibility? SectionTitleVisibility { get; set; }

        public ElementVisibility? ElementVisibility { get; set; }

        public IEnumerable<ContentItemViewModel> Items { get; set; }
    }
}