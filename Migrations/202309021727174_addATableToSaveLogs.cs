namespace StockControll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addATableToSaveLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityType = c.Int(nullable: false),
                        message = c.String(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logs", "User_Id", "dbo.Users");
            DropIndex("dbo.Logs", new[] { "User_Id" });
            DropTable("dbo.Logs");
        }
    }
}
