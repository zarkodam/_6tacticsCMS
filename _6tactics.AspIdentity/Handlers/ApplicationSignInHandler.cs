using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using _6tactics.AspIdentity.Models;

namespace _6tactics.AspIdentity.Handlers
{
    public class ApplicationSignInHandler : SignInManager<ApplicationUser, string>, IApplicationSignInHandler
    {
        public ApplicationSignInHandler(ApplicationUserHandler userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) { }

        public static ApplicationSignInHandler Create(IdentityFactoryOptions<ApplicationSignInHandler> options, IOwinContext context)
        {
            return new ApplicationSignInHandler(context.GetUserManager<ApplicationUserHandler>(), context.Authentication);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserHandler)UserManager);
        }
    }
}