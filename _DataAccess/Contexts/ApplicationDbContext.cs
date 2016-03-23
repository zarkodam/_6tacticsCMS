using _6tactics.Cms.Core.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace _DataAccess.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("6tacticsCMS") { }

        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<Plugin> Plugin { get; set; }
        public DbSet<PluginSettings> PluginSettings { get; set; }
        public DbSet<SeoData> SeoData { get; set; }
        public DbSet<TrackedUser> TrackedUser { get; set; }
        public DbSet<UserActivityTracking> UserActivityTracking { get; set; }

        static ApplicationDbContext()
        {
            // Set create DB initializer
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}