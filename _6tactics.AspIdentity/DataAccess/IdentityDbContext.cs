using Microsoft.AspNet.Identity.EntityFramework;
using _6tactics.AspIdentity.Models;

namespace _6tactics.AspIdentity.DataAccess
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDbContext()
            : base("6tacticsCMS", throwIfV1Schema: false)
        {
        }


        public static IdentityDbContext Create()
        {
            return new IdentityDbContext();
        }
    }
}