using _6tactics.AspIdentity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;

namespace _6tactics.AspIdentity.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<_6tactics.AspIdentity.DataAccess.IdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataAccess.IdentityDbContext context)
        {
            // Create roles
            if (!context.Roles.Any(r => r.Name.Equals("administrators", StringComparison.InvariantCultureIgnoreCase)))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Administrators" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name.Equals("adminreadonly", StringComparison.InvariantCultureIgnoreCase)))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AdminReadOnly" };
                manager.Create(role);
            }


            if (!context.Roles.Any(r => r.Name.Equals("users", StringComparison.InvariantCultureIgnoreCase)))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Users" };
                manager.Create(role);
            }

            // Create admin users
            if (!context.Users.Any(u => u.UserName.Equals("admin", StringComparison.InvariantCultureIgnoreCase)))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "admin", EmailConfirmed = true };
                userManager.Create(userToInsert, "P@ssw0rd");
                userManager.AddToRole(userToInsert.Id, "Administrators");
            }

            if (!context.Users.Any(u => u.UserName.Equals("administrator", StringComparison.InvariantCultureIgnoreCase)))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "administrator", EmailConfirmed = true };
                userManager.Create(userToInsert, "P@ssw0rd");
                userManager.AddToRole(userToInsert.Id, "Administrators");
            }
        }
    }
}
