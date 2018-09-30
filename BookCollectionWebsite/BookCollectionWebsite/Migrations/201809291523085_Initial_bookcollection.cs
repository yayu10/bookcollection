namespace BookCollectionWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_bookcollection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        AuthorID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 80, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 80, unicode: false),
                        Bio = c.String(nullable: false, maxLength: 400, unicode: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorID);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        Image = c.String(),
                        Edition = c.String(maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 400, unicode: false),
                        PublishDate = c.DateTime(nullable: false),
                        ISBN = c.String(nullable: false, maxLength: 13, unicode: false),
                        CategoryID = c.Int(nullable: false),
                        PublisherID = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Publisher", t => t.PublisherID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.PublisherID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Publisher",
                c => new
                    {
                        PublisherID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 120, unicode: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PublisherID);
            
            CreateTable(
                "dbo.BookAuthor",
                c => new
                    {
                        Book_BookID = c.Int(nullable: false),
                        Author_AuthorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_BookID, t.Author_AuthorID })
                .ForeignKey("dbo.Book", t => t.Book_BookID, cascadeDelete: true)
                .ForeignKey("dbo.Author", t => t.Author_AuthorID, cascadeDelete: true)
                .Index(t => t.Book_BookID)
                .Index(t => t.Author_AuthorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Book", "PublisherID", "dbo.Publisher");
            DropForeignKey("dbo.Book", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.BookAuthor", "Author_AuthorID", "dbo.Author");
            DropForeignKey("dbo.BookAuthor", "Book_BookID", "dbo.Book");
            DropIndex("dbo.BookAuthor", new[] { "Author_AuthorID" });
            DropIndex("dbo.BookAuthor", new[] { "Book_BookID" });
            DropIndex("dbo.Book", new[] { "PublisherID" });
            DropIndex("dbo.Book", new[] { "CategoryID" });
            DropTable("dbo.BookAuthor");
            DropTable("dbo.Publisher");
            DropTable("dbo.Category");
            DropTable("dbo.Book");
            DropTable("dbo.Author");
        }
    }
}
