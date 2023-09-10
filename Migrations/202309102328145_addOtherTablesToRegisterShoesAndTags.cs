namespace StockControll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOtherTablesToRegisterShoesAndTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag_HashBarCode = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.Tag_HashBarCode, cascadeDelete: true)
                .Index(t => t.Tag_HashBarCode);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        HashBarCode = c.String(nullable: false, maxLength: 128),
                        Color = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FootSize = c.Int(nullable: false),
                        FootSizeId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.HashBarCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shoes", "Tag_HashBarCode", "dbo.Tags");
            DropIndex("dbo.Shoes", new[] { "Tag_HashBarCode" });
            DropTable("dbo.Tags");
            DropTable("dbo.Shoes");
        }
    }
}
