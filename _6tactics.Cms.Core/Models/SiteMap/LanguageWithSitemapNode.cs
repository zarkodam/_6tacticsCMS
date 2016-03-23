using System.Collections.Generic;

namespace _6tactics.Cms.Core.Models.SiteMap
{
    public class LanguageWithSitemapNode
    {
        public string Language { get; set; }
        public List<SitemapNode> SitemapNodes { get; set; }

        public LanguageWithSitemapNode()
        {
            SitemapNodes = new List<SitemapNode>();
        }
    }
}
