using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Core.ViewModels.FileManager
{
    public class FileManagerCreateViewModel
    {
        public string WhereToCreate { get; set; }

        [Required(ErrorMessage = "If you want to create folder add foldername")]
        [StringLength(30, ErrorMessage = "Should be less than 30")]
        [RegularExpression("^[0-9A-Za-z]+$", ErrorMessage = "Should be 0-9 a-z A-Z without whitespace")]
        public string FolderNameToCreate { get; set; }
    }
}
