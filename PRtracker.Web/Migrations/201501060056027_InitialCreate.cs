using System.Data.Entity.Migrations;

namespace PRTracker.Web.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RawJson = c.String(),
                        ReceivedDate = c.DateTime(nullable: false),
                        RepositoryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Repositories", t => t.RepositoryId)
                .Index(t => t.RepositoryId);
            
            CreateTable(
                "dbo.Repositories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PullRequests",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RepositoryId = c.Int(nullable: false),
                        CreatedByUserId = c.Int(),
                        Number = c.Int(nullable: false),
                        Closed = c.Boolean(nullable: false),
                        ClosedDate = c.DateTime(),
                        Merged = c.Boolean(nullable: false),
                        MergedDate = c.DateTime(),
                        MergedByUserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.MergedByUserId)
                .ForeignKey("dbo.Repositories", t => t.RepositoryId, cascadeDelete: true)
                .Index(t => t.RepositoryId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.MergedByUserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Login = c.String(),
                        DnnLogin = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RepositoryHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Forks = c.Int(nullable: false),
                        Watchers = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        RepositoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Repositories", t => t.RepositoryId, cascadeDelete: true)
                .Index(t => t.RepositoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RepositoryHistories", "RepositoryId", "dbo.Repositories");
            DropForeignKey("dbo.PullRequests", "RepositoryId", "dbo.Repositories");
            DropForeignKey("dbo.PullRequests", "MergedByUserId", "dbo.Users");
            DropForeignKey("dbo.PullRequests", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "RepositoryId", "dbo.Repositories");
            DropIndex("dbo.RepositoryHistories", new[] { "RepositoryId" });
            DropIndex("dbo.PullRequests", new[] { "MergedByUserId" });
            DropIndex("dbo.PullRequests", new[] { "CreatedByUserId" });
            DropIndex("dbo.PullRequests", new[] { "RepositoryId" });
            DropIndex("dbo.Notifications", new[] { "RepositoryId" });
            DropTable("dbo.RepositoryHistories");
            DropTable("dbo.Users");
            DropTable("dbo.PullRequests");
            DropTable("dbo.Repositories");
            DropTable("dbo.Notifications");
        }
    }
}
