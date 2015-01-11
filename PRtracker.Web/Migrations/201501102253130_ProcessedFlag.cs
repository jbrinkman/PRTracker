namespace PRTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcessedFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Processed", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Processed");
        }
    }
}
