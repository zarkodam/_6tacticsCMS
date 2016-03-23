using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Services.Web;
using _DataAccess.UnitOfWork;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    public class WebController : BaseWebController
    {
        public WebController(IUnitOfWork unitOfWork, ISeoDataService seoData, ISiteMapService siteMap)
        {
            SiteMap = siteMap;
            Uof = unitOfWork;
            SeoDataService = seoData;
        }

        [Route("robots.txt", Name = "GetRobotsText"), OutputCache(Duration = 21600)] // 6h
        public new ContentResult RobotsText()
        {
            return base.RobotsText();
        }

        [Route("sitemap.xml", Name = "GetSitemapXml"), OutputCache(Duration = 21600)] // 6h
        public new ContentResult SitemapXml()
        {
            return base.SitemapXml();
        }

        public new ActionResult GetFile(string folderPath, string filename, string extensionName)
        {
            return base.GetFile(folderPath, filename, extensionName);
        }

        [ChildActionOnlyCustom]
        public new PartialViewResult SeoData()
        {
            return base.SeoData();
        }

        public new ActionResult Index(string lang, string routeTitle)
        {
            return base.Index(lang, routeTitle);
        }
    }
}