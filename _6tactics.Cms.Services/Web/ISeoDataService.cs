using _6tactics.Cms.Core.ViewModels.Web;

namespace _6tactics.Cms.Services.Web
{
    public interface ISeoDataService
    {
        void SetSeoData(string keywords, string description, string logoFbUrl);
        SeoViewModel GetSeoData();
    }
}