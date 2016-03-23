namespace _6tactics.Cms.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeoData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeoData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Keywords = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SeoData");
        }
    }
}
