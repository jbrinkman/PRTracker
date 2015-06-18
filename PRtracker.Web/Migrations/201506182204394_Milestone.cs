namespace PRTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Milestone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PullRequests", "Milestone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PullRequests", "Milestone");
        }
    }
}
