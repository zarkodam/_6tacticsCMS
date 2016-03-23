namespace _6tactics.Cms.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackedUserAndUserActivityTracking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrackedUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsersIp = c.String(),
                        UsersDns = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserActivityTracking",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        TrackedUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TrackedUser", t => t.TrackedUserId, cascadeDelete: true)
                .Index(t => t.TrackedUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserActivityTracking", "TrackedUserId", "dbo.TrackedUser");
            DropIndex("dbo.UserActivityTracking", new[] { "TrackedUserId" });
            DropTable("dbo.UserActivityTracking");
            DropTable("dbo.TrackedUser");
        }
    }
}
