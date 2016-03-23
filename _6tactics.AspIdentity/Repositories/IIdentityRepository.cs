using _6tactics.AspIdentity.Handlers;

namespace _6tactics.AspIdentity.Repositories
{
    public interface IIdentityRepository
    {
        IApplicationUserHandler UserManager { get; set; }
        IApplicationRoleHandler RoleManager { get; set; }
        IApplicationSignInHandler SignInManager { get; set; }
        void Dispose(IIdentityRepository identityRepository, bool disposing);
    }
}