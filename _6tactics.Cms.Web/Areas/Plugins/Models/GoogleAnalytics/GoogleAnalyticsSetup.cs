using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Web.Areas.Plugins.Models.GoogleAnalytics
{
    public class GoogleAnalyticsSetup
    {
        public bool IsIncluded { get; set; }

        [Required]
        public string TrackingId { get; set; }

        public GoogleAnalyticsSetup()
        {
            IsIncluded = true;
        }
    }
}