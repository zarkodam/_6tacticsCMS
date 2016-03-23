using _6tactics.AspIdentity.ViewModels.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace _6tactics.AspIdentity.ViewModels.UsersHandler
{
    public class UsersHandlerCreateViewModel
    {
        public RegisterViewModel RegisterViewModel { get; set; }

        public List<IdentityRole> SelectedRoles { get; set; }
    }
}
