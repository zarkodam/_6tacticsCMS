using _6tactics.Cms.Core.ViewModels.Admin;

namespace _6tactics.Cms.Services.Admin
{
    public interface ISetupService
    {
        SetupViewModel BuildSetupViewModel();
        void SaveSetup(SetupViewModel setupViewModel);
    }
}