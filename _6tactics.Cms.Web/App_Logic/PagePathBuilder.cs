using System.Collections.Generic;
using System.Linq;
using _6tactics.Cms.Core.Enums;
using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Models;
using _6tactics.Cms.Core.Models.Common;

namespace _6tactics.Cms.Web.App_Logic
{
    public class PagePathBuilder
    {
        private readonly List<string> _pageList;
        private readonly string _pathDevider;

        private void PageStringPathBuilder(string selectedLanguage, IEnumerable<IContentItem> contentItems)
        {
            foreach (var page in contentItems.Where(item => item.ContentType == ContentType.Page))
            {
                if (!ElementsVisibilityUtility.IsPageSelected(page, ElementsVisibilityUtility.CurrentRouteTitle)) continue;

                if (page.ContentItems.Count(i => i.ContentType == ContentType.Page) > 0)
                    PageStringPathBuilder(selectedLanguage, page.ContentItems);

                _pageList.Add(page.Title);
            }
        }

        public PagePathBuilder(string selectedLanguage, IEnumerable<IContentItem> contentItems, string pathDevider = " / ")
        {
            _pageList = new List<string>();
            _pathDevider = pathDevider;
            PageStringPathBuilder(selectedLanguage, contentItems);
        }


        public string GetPagePath
        {
            get
            {
                string pagePath = "";
                _pageList.Reverse();

                if (_pageList.Count <= 1) return pagePath;

                int i = 1;

                foreach (string page in _pageList)
                {
                    pagePath += i < _pageList.Count ? string.Concat(page, _pathDevider) : page;
                    i++;
                }

                return pagePath;
            }
        }
    }
}