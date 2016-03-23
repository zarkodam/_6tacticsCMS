using _6tactics.Cms.Core.Models.Common;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.ViewModels.Admin
{
    public class AdminPagesListViewModel
    {
        public string SelectedLanguage { get; set; }
        public string CurrentAction { get; set; }
        public IEnumerable<ContentItemWithParentCount> ElementsDepthAndTitle { get; set; }
    }
}
