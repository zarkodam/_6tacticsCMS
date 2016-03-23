using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.ViewModels.Common;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.ViewModels.Web
{
    public class PageViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public ProjectViewModel Project { get; set; }
        public SectionViewModel MainMenu { get; set; }
        public SectionViewModel PageContentMenu { get; set; }
        public IDictionary<string, string> LanguagesWithDefaultRouteTitle { get; set; }
        public IEnumerable<SectionViewModel> PageSections { get; set; }
        public ContentItem Footer { get; set; }
        public IEnumerable<SectionViewModel> FooterSections { get; set; }
        public bool IsAnyGoogleMapOnThisPage { get; set; }
    }
}