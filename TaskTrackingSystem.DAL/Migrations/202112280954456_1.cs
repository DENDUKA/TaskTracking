namespace TaskTrackingSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "Post", c => c.String(maxLength: 100));
            AddColumn("dbo.Account", "Wage", c => c.Int(nullable: false));
            AddColumn("dbo.Account", "Department", c => c.String(maxLength: 100));
            AddColumn("dbo.Account", "DateBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.Account", "Education", c => c.String());
            AddColumn("dbo.Account", "Responsibilities", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Account", "Responsibilities");
            DropColumn("dbo.Account", "Education");
            DropColumn("dbo.Account", "DateBirth");
            DropColumn("dbo.Account", "Department");
            DropColumn("dbo.Account", "Wage");
            DropColumn("dbo.Account", "Post");
        }
    }
}
