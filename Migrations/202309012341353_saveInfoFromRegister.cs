namespace StockControll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saveInfoFromRegister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Users", "DeletedAt", c => c.DateTime());
            AlterColumn("dbo.Users", "CPF", c => c.String(maxLength: 11, unicode: false));
            DropColumn("dbo.Users", "Login");
            DropColumn("dbo.Users", "PasswordResetToken");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PasswordResetToken", c => c.String(maxLength: 250, unicode: false));
            AddColumn("dbo.Users", "Login", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "CPF", c => c.String(maxLength: 20, unicode: false));
            DropColumn("dbo.Users", "DeletedAt");
            DropColumn("dbo.Users", "Email");
        }
    }
}
