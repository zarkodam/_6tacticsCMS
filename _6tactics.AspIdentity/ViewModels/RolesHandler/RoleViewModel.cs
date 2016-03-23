using System.ComponentModel.DataAnnotations;

namespace _6tactics.AspIdentity.ViewModels.RolesHandler
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }
}