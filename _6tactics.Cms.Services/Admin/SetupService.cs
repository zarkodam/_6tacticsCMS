using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.ViewModels.Admin;
using _6tactics.Cms.Core.ViewModels.Common;
using _6tactics.Cms.Services.Web;
using _6tactics.Utilities.ObjectMapping;
using _DataAccess.UnitOfWork;

namespace _6tactics.Cms.Services.Admin
{
    public class SetupService : ISetupService
    {
        private readonly IUnitOfWork _uof;
        private readonly ISeoDataService _seoData;

        public SetupService(IUnitOfWork uof, ISeoDataService seoData)
        {
            _uof = uof;
            _seoData = seoData;
        }

        public SetupViewModel BuildSetupViewModel()
        {
            ContentItem projectContentItem = _uof.ContentItemRepo.ProjectItem;
            ProjectViewModel projectViewModel = ObjectMapper.Map<ContentItem, ProjectViewModel>(projectContentItem);

            return new SetupViewModel
            {
                ProjectContentItem = projectViewModel,
                Seo = _seoData.GetSeoData()
            };
        }

        public void SaveSetup(SetupViewModel setupViewModel)
        {
            ContentItem contentItemModel = ObjectMapper.Map<ProjectViewModel, ContentItem>(setupViewModel.ProjectContentItem);

            if (setupViewModel.ProjectContentItem.Id != null)
                _uof.ContentItemRepo.UpdateContentItem(contentItemModel);
            else
                _uof.ContentItemRepo.InsertContentItem(contentItemModel);

            _uof.Save();

            _seoData.SetSeoData(setupViewModel.Seo.Keywords, setupViewModel.Seo.Description, setupViewModel.Seo.LogoFbUrl);
        }
    }
}
