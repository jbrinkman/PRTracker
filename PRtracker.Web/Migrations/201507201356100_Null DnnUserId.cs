namespace PRTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullDnnUserId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "DnnUserId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "DnnUserId", c => c.Int(nullable: false));
        }
    }
}
