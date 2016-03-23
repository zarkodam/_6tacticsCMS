using System.ComponentModel.DataAnnotations;

namespace _6tactics.AspIdentity.ViewModels.Manage
{
    public class ChangeEmailViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}