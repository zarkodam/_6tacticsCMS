namespace _6tactics.Cms.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeoDataUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeoData", "LogoFbUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SeoData", "LogoFbUrl");
        }
    }
}
