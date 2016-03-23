using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.ViewModels.Common;

namespace _6tactics.Cms.Core.ViewModels.Admin
{
    public class AdminActionsViewModel
    {
        public CreateEditDataViewModel CreateEditDataViewModel { get; set; }
        public ContentItemViewModel ContentItemViewModel { get; set; }
        public ContentItem ContentItemModel { get; set; }
        public StatisticsViewModel StatisticsViewModel { get; set; }

    }
}
