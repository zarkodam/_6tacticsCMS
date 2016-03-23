using System.Web;
using Microsoft.AspNet.Identity.Owin;
using _6tactics.AspIdentity.Handlers;

namespace _6tactics.AspIdentity.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private IApplicationUserHandler _userManager;

        public IApplicationUserHandler UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationUserHandler>();
            }
            set { _userManager = value; }
        }

        private IApplicationRoleHandler _roleManager;

        public IApplicationRoleHandler RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationRoleHandler>();
            }
            set { _roleManager = value; }
        }

        private IApplicationSignInHandler _signInManager;

        public IApplicationSignInHandler SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInHandler>();
            }
            set { _signInManager = value; }
        }

        public void Dispose(IIdentityRepository identityRepository, bool disposing)
        {
            if (!disposing) return;

            if (identityRepository.SignInManager != null)
            {
                identityRepository.SignInManager.Dispose();
                identityRepository.SignInManager = null;
            }

            if (identityRepository.UserManager != null)
            {
                identityRepository.UserManager.Dispose();
                identityRepository.UserManager = null;
            }

            if (identityRepository.RoleManager != null)
            {
                identityRepository.RoleManager.Dispose();
                identityRepository.RoleManager = null;
            }
        }
    }
}
