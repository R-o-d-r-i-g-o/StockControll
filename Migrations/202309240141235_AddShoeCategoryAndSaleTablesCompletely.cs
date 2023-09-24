namespace StockControll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShoeCategoryAndSaleTablesCompletely : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shoes", "Tag_HashBarCode", "dbo.Tags");
            DropIndex("dbo.Shoes", new[] { "Tag_HashBarCode" });
            DropPrimaryKey("dbo.Shoes");
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Color = c.String(nullable: false),
                        Sole = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Shoe_BarCodeHash = c.String(nullable: false, maxLength: 128),
                        Seller_Id = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shoe_BarCodeHash, t.Seller_Id })
                .ForeignKey("dbo.Users", t => t.Seller_Id, cascadeDelete: true)
                .ForeignKey("dbo.Shoes", t => t.Shoe_BarCodeHash, cascadeDelete: true)
                .Index(t => t.Shoe_BarCodeHash)
                .Index(t => t.Seller_Id);
            
            AddColumn("dbo.Shoes", "BarCodeHash", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Shoes", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Shoes", "Size", c => c.Int(nullable: false));
            AddColumn("dbo.Shoes", "SizeId", c => c.String(nullable: false));
            AddColumn("dbo.Shoes", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Shoes", "Category_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Shoes", "BarCodeHash");
            CreateIndex("dbo.Shoes", "Category_Id");
            AddForeignKey("dbo.Shoes", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            DropColumn("dbo.Shoes", "Id");
            DropColumn("dbo.Shoes", "Tag_HashBarCode");
            DropTable("dbo.Tags");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.Shoes", "Tag_HashBarCode", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Shoes", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Sales", "Shoe_BarCodeHash", "dbo.Shoes");
            DropForeignKey("dbo.Shoes", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Sales", "Seller_Id", "dbo.Users");
            DropIndex("dbo.Shoes", new[] { "Category_Id" });
            DropIndex("dbo.Sales", new[] { "Seller_Id" });
            DropIndex("dbo.Sales", new[] { "Shoe_BarCodeHash" });
            DropPrimaryKey("dbo.Shoes");
            DropColumn("dbo.Shoes", "Category_Id");
            DropColumn("dbo.Shoes", "CreatedAt");
            DropColumn("dbo.Shoes", "SizeId");
            DropColumn("dbo.Shoes", "Size");
            DropColumn("dbo.Shoes", "Price");
            DropColumn("dbo.Shoes", "BarCodeHash");
            DropTable("dbo.Sales");
            DropTable("dbo.Categories");
            AddPrimaryKey("dbo.Shoes", "Id");
            CreateIndex("dbo.Shoes", "Tag_HashBarCode");
            AddForeignKey("dbo.Shoes", "Tag_HashBarCode", "dbo.Tags", "HashBarCode", cascadeDelete: true);
        }
    }
}
