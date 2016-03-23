using _6tactics.Cms.Core.Models.Admin;
using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Web.Areas.Plugins.Models.PostAbusing
{
    public class PostAbusingSettings : IPostAbusingSettings
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "Minimum value can be 1"), Display(Name = "Number of allowed attempts")]
        public int NumberOfAllowedAttempts { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Minimum value can be 1"), Display(Name = "Time per allowed attempts")]
        public int TimePerAllowedAttemptsInHours { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Minimum value can be 1"), Display(Name = "Maximum time of ip blocking")]
        public int MaxBlockingTimeInHours { get; set; }

        public PostAbusingSettings()
        {
            NumberOfAllowedAttempts = 5;
            TimePerAllowedAttemptsInHours = 1;
            MaxBlockingTimeInHours = 24;
        }

    }
}