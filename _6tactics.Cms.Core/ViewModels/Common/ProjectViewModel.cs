using _6tactics.Cms.Core.Enums.Admin;
using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Core.ViewModels.Common
{
    public class ProjectViewModel
    {
        public int? Id { get; set; }
        public int Priority { get; set; }

        [StringLength(65, MinimumLength = 3, ErrorMessage = "Keywords cannot be less than 3 and longer than 65 characters!")]
        [Display(Name = "Website name"), Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        //[StringLength(70, ErrorMessage = "Keywords cannot be longer than 70 characters!")]
        [Display(Name = "Website logo")]
        public string FileUrl { get; set; }

        public ContentType ContentType { get; set; }

        public ProjectViewModel()
        {
            Priority = 0;
            ContentType = ContentType.Project;
        }
    }
}
