using _6tactics.SimpleCaptcha;
using System.ComponentModel.DataAnnotations;

namespace _6tactics.AspIdentity.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Captcha")]
        public string Captcha { get; set; }

        [Required(ErrorMessage = "You have to solve the problem!")]
        [Display(Name = "Calculate")]
        [SimpleCaptchaValidator("Captcha", ErrorMessage = "Result is not valid!")]
        public string CaptchaCode { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}