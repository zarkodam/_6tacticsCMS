using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.ViewModels.Web;
using _6tactics.Utilities.ObjectMapping;
using _DataAccess.Repositories;
using _DataAccess.UnitOfWork;
using System.Linq;

namespace _6tactics.Cms.Services.Web
{
    public class SeoDataService : ISeoDataService
    {
        private readonly IUnitOfWork _uof;
        private readonly IGenericRepository<SeoData> _seoDataRepo;

        public SeoDataService(IUnitOfWork uof)
        {
            _uof = uof;
            _seoDataRepo = uof.GenericRepo<SeoData>();
        }

        public void SetSeoData(string keywords, string description, string logoFbUrl)
        {
            if (_seoDataRepo.Get().Any())
            {
                SeoData seoData = _seoDataRepo.Get().First();
                seoData.LogoFbUrl = logoFbUrl;
                seoData.Keywords = keywords;
                seoData.Description = description;
            }
            else
            {
                _seoDataRepo.Insert(new SeoData
                {
                    LogoFbUrl = logoFbUrl,
                    Keywords = keywords,
                    Description = description
                });
            }

            _uof.Save();
        }

        public SeoViewModel GetSeoData()
        {
            ContentItem projectItem = _uof.ContentItemRepo.ProjectItem;
            var seoViewModel = new SeoViewModel();

            if (projectItem == null || !_seoDataRepo.Get().Any()) return seoViewModel;

            seoViewModel = ObjectMapper.Map<SeoData, SeoViewModel>(_seoDataRepo.Get().First());
            seoViewModel.WebsiteName = projectItem.Title;
            seoViewModel.WebsiteLogo = projectItem.FileUrl;

            return seoViewModel;
        }
    }
}
