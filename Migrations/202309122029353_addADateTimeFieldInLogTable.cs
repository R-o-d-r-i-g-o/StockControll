namespace StockControll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addADateTimeFieldInLogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "CreatedAt");
        }
    }
}
