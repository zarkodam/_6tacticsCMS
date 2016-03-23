using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _6tactics.AspIdentity.ViewModels.UsersHandler
{
    public class UsersHandlerEditViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }
        public bool ResetPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}
