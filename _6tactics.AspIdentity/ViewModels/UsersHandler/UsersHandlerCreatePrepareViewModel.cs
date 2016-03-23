using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _6tactics.AspIdentity.ViewModels.UsersHandler
{
    public class UsersHandlerCreatePrepareViewModel
    {
        public UserPersonalDataViewModel UserPersonalData { get; set; }

        public List<IdentityRole> SelectedRoles { get; set; }
    }
}
