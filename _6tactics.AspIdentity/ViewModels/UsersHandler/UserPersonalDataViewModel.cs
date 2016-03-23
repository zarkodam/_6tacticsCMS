using System.ComponentModel.DataAnnotations;

namespace _6tactics.AspIdentity.ViewModels.UsersHandler
{
    public class UserPersonalDataViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
