namespace WebBanQuanAo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProductCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductCategory", "Alias", c => c.String(nullable: false, maxLength: 150));
            AddColumn("dbo.ProductCategory", "SeoTitle", c => c.String(maxLength: 250));
            AddColumn("dbo.ProductCategory", "SeoDescription", c => c.String(maxLength: 500));
            AddColumn("dbo.ProductCategory", "SeoKeywords", c => c.String(maxLength: 250));
            AlterColumn("dbo.ProductCategory", "Icon", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductCategory", "Icon", c => c.String());
            DropColumn("dbo.ProductCategory", "SeoKeywords");
            DropColumn("dbo.ProductCategory", "SeoDescription");
            DropColumn("dbo.ProductCategory", "SeoTitle");
            DropColumn("dbo.ProductCategory", "Alias");
        }
    }
}
