Enable migration:

	Enable-Migrations -ContextProjectName _6tactics.AspIdentity -StartUpProjectName _6tactics.Cms.Web -ContextTypeName _6tactics.AspIdentity.DataAccess.IdentityDbContext -ProjectName _6tactics.Cms.Web

Seed method: 

	protected override void Seed(_6tactics.AspIdentity.DataAccess.IdentityDbContext context)
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


- created Administrator role and Administrator and Admin users, which are added in Administrator role

Enable-Migrations -ContextProjectName _DataAccess -StartUpProjectName _6tactics.Cms.Web -ContextTypeName _DataAccess.Contexts.ApplicationDbContext -ProjectName _6tactics.Cms.Web