namespace PRTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIsEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsEmployee", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "DnnUserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DnnUserId");
            DropColumn("dbo.Users", "IsEmployee");
        }
    }
}
