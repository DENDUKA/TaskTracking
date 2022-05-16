namespace TaskTrackingSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PayLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(maxLength: 100),
                        Date = c.DateTime(nullable: false),
                        Json = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.Login)
                .Index(t => t.Login);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PayLists", "Login", "dbo.Account");
            DropIndex("dbo.PayLists", new[] { "Login" });
            DropTable("dbo.PayLists");
        }
    }
}
