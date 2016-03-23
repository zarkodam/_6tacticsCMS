using _6tactics.AspIdentity.Models;
using _6tactics.AspIdentity.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityDbContext = _6tactics.AspIdentity.DataAccess.IdentityDbContext;

namespace _6tactics.AspIdentity.Handlers
{
    public class ApplicationUserHandler : UserManager<ApplicationUser>, IApplicationUserHandler
    {
        public ApplicationUserHandler(IUserStore<ApplicationUser> store) : base(store) { }

        public static ApplicationUserHandler Create(IdentityFactoryOptions<ApplicationUserHandler> options, IOwinContext context)
        {
            var manager = new ApplicationUserHandler(new UserStore<ApplicationUser>(context.Get<IdentityDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };


            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            //manager.PasswordValidator = new CustomPasswordValidator(6);

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses 
            // Phone and Emails as a step of receiving a code for verifying 
            // the user You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });

            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });

            //sendGridNetworkCredential
            manager.EmailService = new SendGridEmailService();
            //manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                { TokenLifespan = TimeSpan.FromHours(3) };

            return manager;
        }


        public virtual async Task<IdentityResult> AddUserToRolesAsync(string userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, string>)Store;

            ApplicationUser user = await FindByIdAsync(userId).ConfigureAwait(false);

            if (user == null) throw new InvalidOperationException("Invalid user Id");

            IList<string> userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);

            // Add user to each role using UserRoleStore
            foreach (var role in roles.Where(role => !userRoles.Contains(role)))
                await userRoleStore.AddToRoleAsync(user, role).ConfigureAwait(false);

            // Call update once when all roles are added
            return await UpdateAsync(user).ConfigureAwait(false);
        }


        public virtual async Task<IdentityResult> RemoveUserFromRolesAsync(string userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, string>)Store;

            ApplicationUser user = await FindByIdAsync(userId).ConfigureAwait(false);

            if (user == null) throw new InvalidOperationException("Invalid user Id");

            IList<string> userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);

            // Remove user to each role using UserRoleStore
            foreach (var role in roles.Where(userRoles.Contains))
                await userRoleStore.RemoveFromRoleAsync(user, role).ConfigureAwait(false);

            // Call update once when all roles are removed
            return await UpdateAsync(user).ConfigureAwait(false);
        }
    }
}