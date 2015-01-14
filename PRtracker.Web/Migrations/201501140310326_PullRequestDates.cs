namespace PRTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PullRequestDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PullRequests", "CreatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PullRequests", "CreatedDate");
        }
    }
}
