using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using IdentityDbContext = _6tactics.AspIdentity.DataAccess.IdentityDbContext;

namespace _6tactics.AspIdentity.Handlers
{
    public class ApplicationRoleHandler : RoleManager<IdentityRole>, IApplicationRoleHandler
    {
        public ApplicationRoleHandler(IRoleStore<IdentityRole, string> roleStore) : base(roleStore) { }

        public static ApplicationRoleHandler Create(IdentityFactoryOptions<ApplicationRoleHandler> options, IOwinContext context)
        {
            return new ApplicationRoleHandler(new RoleStore<IdentityRole>(context.Get<IdentityDbContext>()));
        }
    }
}