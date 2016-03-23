using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.ViewModels.Admin;

namespace _6tactics.Cms.Services.Admin
{
    public interface IAdminViewModelBuilderService
    {
        AdminActionsViewModel CreateAdminActionsViewModel(
            int? id,
            ContentItemAction contentItemAction,
            string language = null,
            StatisticsViewModel statisticsViewModel = null);

        AdminActionsViewModel CreateAdminActionsViewModel(string language, int? pageId, string uploadFolderPath);
    }
}