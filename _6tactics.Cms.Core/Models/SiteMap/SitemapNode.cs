using _6tactics.Cms.Core.Enums.Web;
using System;

namespace _6tactics.Cms.Core.Models.SiteMap
{
    public class SitemapNode
    {
        public SitemapFrequency? Frequency { get; set; }
        public DateTime? LastModified { get; set; }
        public double? Priority { get; set; }
        public string Url { get; set; }
    }
}
