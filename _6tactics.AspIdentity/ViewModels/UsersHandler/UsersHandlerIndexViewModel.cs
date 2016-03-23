using _6tactics.AspIdentity.Models;
using System.Collections.Generic;

namespace _6tactics.AspIdentity.ViewModels.UsersHandler
{
    public class UsersHandlerIndexViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public List<string> RolesList { get; set; }

        public UsersHandlerIndexViewModel()
        {
            RolesList = new List<string>();
        }
    }
}
