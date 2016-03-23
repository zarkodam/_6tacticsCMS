Migration from a separate project:

	Enable-Migrations -EnableAutomaticMigrations -ProjectName _6tactics.AspIdentity -StartupProjectName AspIdentityExample
	 - use migrations from separated project in separated project

	Enable-Migrations -ContextProjectName _6tactics.AspIdentity -StartUpProjectName AspIdentityExample -ContextTypeName _6tactics.AspIdentity.DataAccess.IdentityDbContext -ProjectName AspIdentityExample
	 - use migrations from separated project in default(main) project

	Enable-Migrations -ProjectName _6tactics.AspIdentity -StartUpProjectName AspIdentityExample -Verbose

