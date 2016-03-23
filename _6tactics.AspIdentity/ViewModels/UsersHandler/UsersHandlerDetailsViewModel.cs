using _6tactics.AspIdentity.Models;
using System.Collections.Generic;

namespace _6tactics.AspIdentity.ViewModels.UsersHandler
{
    public class UsersHandlerDetailsViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
    }
}
