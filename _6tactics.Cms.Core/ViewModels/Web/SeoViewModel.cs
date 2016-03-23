using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Core.ViewModels.Web
{
    public class SeoViewModel
    {
        public string WebsiteName { get; set; }
        public string WebsiteLogo { get; set; }

        [Display(Name = "Facebook logo")]
        [Required(ErrorMessage = "Facebook logo url is required! Recommended dimension is 200px * 200px")]
        public string LogoFbUrl { get; set; }

        [Required(ErrorMessage = "Keywords are required!")]
        [StringLength(200, MinimumLength = 100, ErrorMessage = "Keywords cannot be less than 100 and longer than 200 characters! 150 is recommended.")]
        public string Keywords { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [StringLength(200, MinimumLength = 100, ErrorMessage = "Description cannot be less than 100 and longer than 200 characters! 150 is recommended.")]
        public string Description { get; set; }
    }
}
