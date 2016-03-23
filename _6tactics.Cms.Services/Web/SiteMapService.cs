using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Models.Common;
using _6tactics.Cms.Core.Models.SiteMap;
using _6tactics.Cms.Services.Common;
using _DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Xml.Linq;

namespace _6tactics.Cms.Services.Web
{
    public class SiteMapService : ISiteMapService
    {
        #region Fields

        private readonly IContentItemsWithParentCountService _contentItemsWithParentCount;

        #endregion


        #region Cotr

        public SiteMapService(IContentItemRepository contentItem, IContentItemsWithParentCountService contentItemsWithParentCount)
        {
            _contentItemsWithParentCount = contentItemsWithParentCount;
        }

        #endregion


        #region Actions

        public string GetSiteMap()
        {
            return CreateSitemapDocument(GetSiteMapNodes());
        }

        #endregion


        #region Helpers

        private static double CalculatePriority(int parentCount)
        {
            //n starts from 1
            // number 1 has to be 1
            // number 2 has to be 0.9
            // number 3 has to be 0.8
            // ....
            // number 9 has to be 0.1
            // number 10 has to be 0.1
            // number 11 has to be 0.1
            //(10 - (n - 1)) / 10

            int n = (parentCount >= 10 ? 10 : parentCount) - 1;
            return (double)(10 - n) / 10;
        }

        private static string UrlBuilder(string lang, string pageRoute)
        {
            return string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), "/web/index/", lang, "/", pageRoute);
        }

        private IEnumerable<LanguageWithSitemapNode> GetSiteMapNodes()
        {
            Dictionary<ContentItem, List<ContentItemWithParentCount>> pagesGroupedByLanguage =
                _contentItemsWithParentCount.GetFilteredByRootAndChildAndGroupedByRoot(
                    i => i.ContentType.Equals(ContentType.Language),
                    i => i.ContentType.Equals(ContentType.Page) && i.ElementVisibility.Equals(ElementVisibility.Visible));

            var pagesByLanguages = new List<LanguageWithSitemapNode>();

            foreach (var lang in pagesGroupedByLanguage)
            {
                var languageWithPages = new LanguageWithSitemapNode { Language = lang.Key.Title };
                foreach (var page in lang.Value)
                {
                    languageWithPages.SitemapNodes.Add(new SitemapNode
                    {
                        LastModified = DateTime.Now,
                        Priority = CalculatePriority(page.Depth),
                        Url = UrlBuilder(lang.Key.Title, page.ContentItem.RouteTitle)
                    });
                }

                pagesByLanguages.Add(languageWithPages);
            }

            return pagesByLanguages;
        }

        private static string CreateSitemapDocument(IEnumerable<LanguageWithSitemapNode> languageWithSitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var root = new XElement(xmlns + "urlset");

            foreach (var languageWithSitemapNode in languageWithSitemapNodes)
                foreach (SitemapNode sitemapNode in languageWithSitemapNode.SitemapNodes)
                {
                    var urlElement = new XElement(
                        xmlns + "url",
                        new XElement(string.Concat(xmlns + "loc"), Uri.EscapeUriString(sitemapNode.Url)),

                        sitemapNode.LastModified == null ? null : new XElement(
                            string.Concat(xmlns + "lastmod"),
                            sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),

                        sitemapNode.Frequency == null ? null : new XElement(
                            string.Concat(xmlns + "changefreq"),
                            sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),

                        sitemapNode.Priority == null ? null : new XElement(
                            string.Concat(xmlns + "priority"),
                            sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));

                    root.Add(urlElement);
                }

            return new XDocument(root).ToString();
        }

        #endregion
    }
}
