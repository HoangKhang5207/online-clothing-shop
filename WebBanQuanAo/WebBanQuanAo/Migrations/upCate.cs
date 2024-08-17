namespace WebBanQuanAo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upCate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Category", "Alias", c => c.String(maxLength: 150));
            AlterColumn("dbo.ProductCategory", "Alias", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductCategory", "Alias", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Category", "Alias", c => c.String());
        }
    }
}
