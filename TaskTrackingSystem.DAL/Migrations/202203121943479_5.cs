namespace TaskTrackingSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PCNetworkInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountLogin = c.String(maxLength: 100),
                        Ip = c.String(),
                        MAC = c.String(),
                        PCName = c.String(),
                        IsOnline = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountLogin)
                .Index(t => t.AccountLogin);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PCNetworkInfoes", "AccountLogin", "dbo.Account");
            DropIndex("dbo.PCNetworkInfoes", new[] { "AccountLogin" });
            DropTable("dbo.PCNetworkInfoes");
        }
    }
}
