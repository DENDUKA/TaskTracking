namespace TaskTrackingSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Account", "Wage", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Account", "Wage", c => c.Int(nullable: false));
        }
    }
}
