namespace _6tactics.Cms.Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PluginsSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Plugin",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PluginName = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.PluginSettings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SettingsName = c.String(),
                    SettingsJsonString = c.String(),
                    Plugin_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plugin", t => t.Plugin_Id)
                .Index(t => t.Plugin_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PluginSettings", "Plugin_Id", "dbo.Plugin");
            DropIndex("dbo.PluginSettings", new[] { "Plugin_Id" });
            DropTable("dbo.PluginSettings");
            DropTable("dbo.Plugin");
        }
    }
}
