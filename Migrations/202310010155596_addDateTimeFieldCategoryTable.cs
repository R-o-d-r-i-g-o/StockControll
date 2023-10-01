namespace StockControll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDateTimeFieldCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "CreatedAt");
        }
    }
}
