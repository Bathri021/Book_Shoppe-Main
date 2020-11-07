namespace Book_Shoppe.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigrationV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 55),
                        Author = c.String(nullable: false, maxLength: 26),
                        GenreID = c.Int(nullable: false),
                        LanguageID = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        NoOfPages = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.Genres", t => t.GenreID, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.Title, unique: true)
                .Index(t => t.GenreID)
                .Index(t => t.LanguageID);
            
            CreateTable(
                "dbo.CartBooks",
                c => new
                    {
                        CartBookID = c.Int(nullable: false, identity: true),
                        CartID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartBookID)
                .ForeignKey("dbo.Book", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Carts", t => t.CartID, cascadeDelete: true)
                .Index(t => t.CartID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        CartRate = c.Int(nullable: false),
                        IsOrdered = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CartID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: false)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        RoleID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 26),
                        UserName = c.String(nullable: false, maxLength: 26),
                        MailID = c.String(nullable: false, maxLength: 64),
                        Password = c.String(nullable: false, maxLength: 12),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.MailID, unique: true);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 8),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        WishListID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WishListID)
                .ForeignKey("dbo.Book", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: false)
                .Index(t => t.UserID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreID = c.Int(nullable: false, identity: true),
                        GenreName = c.String(nullable: false, maxLength: 16),
                    })
                .PrimaryKey(t => t.GenreID)
                .Index(t => t.GenreName, unique: true);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        LanguageID = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(nullable: false, maxLength: 16),
                    })
                .PrimaryKey(t => t.LanguageID)
                .Index(t => t.LanguageName, unique: true);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        CartID = c.Int(nullable: false),
                        ShipmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Carts", t => t.CartID, cascadeDelete: true)
                .ForeignKey("dbo.Shipments", t => t.ShipmentID, cascadeDelete: true)
                .Index(t => t.CartID)
                .Index(t => t.ShipmentID);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        ShipmentID = c.Int(nullable: false, identity: true),
                        Address = c.String(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShipmentID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateStoredProcedure(
                "dbo.User_Insert",
                p => new
                    {
                        RoleID = p.Int(),
                        Name = p.String(maxLength: 26),
                        UserName = p.String(maxLength: 26),
                        MailID = p.String(maxLength: 64),
                        Password = p.String(maxLength: 12),
                    },
                body:
                    @"INSERT [dbo].[User]([RoleID], [Name], [UserName], [MailID], [Password])
                      VALUES (@RoleID, @Name, @UserName, @MailID, @Password)
                      
                      DECLARE @UserID int
                      SELECT @UserID = [UserID]
                      FROM [dbo].[User]
                      WHERE @@ROWCOUNT > 0 AND [UserID] = scope_identity()
                      
                      SELECT t0.[UserID]
                      FROM [dbo].[User] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[UserID] = @UserID"
            );
            
            CreateStoredProcedure(
                "dbo.User_Update",
                p => new
                    {
                        UserID = p.Int(),
                        RoleID = p.Int(),
                        Name = p.String(maxLength: 26),
                        UserName = p.String(maxLength: 26),
                        MailID = p.String(maxLength: 64),
                        Password = p.String(maxLength: 12),
                    },
                body:
                    @"UPDATE [dbo].[User]
                      SET [RoleID] = @RoleID, [Name] = @Name, [UserName] = @UserName, [MailID] = @MailID, [Password] = @Password
                      WHERE ([UserID] = @UserID)"
            );
            
            CreateStoredProcedure(
                "dbo.User_Delete",
                p => new
                    {
                        UserID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[User]
                      WHERE ([UserID] = @UserID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.User_Delete");
            DropStoredProcedure("dbo.User_Update");
            DropStoredProcedure("dbo.User_Insert");
            DropForeignKey("dbo.Orders", "ShipmentID", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "UserID", "dbo.User");
            DropForeignKey("dbo.Orders", "CartID", "dbo.Carts");
            DropForeignKey("dbo.Book", "UserID", "dbo.User");
            DropForeignKey("dbo.Book", "LanguageID", "dbo.Languages");
            DropForeignKey("dbo.Book", "GenreID", "dbo.Genres");
            DropForeignKey("dbo.Carts", "UserID", "dbo.User");
            DropForeignKey("dbo.WishLists", "UserID", "dbo.User");
            DropForeignKey("dbo.WishLists", "BookID", "dbo.Book");
            DropForeignKey("dbo.User", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.CartBooks", "CartID", "dbo.Carts");
            DropForeignKey("dbo.CartBooks", "BookID", "dbo.Book");
            DropIndex("dbo.Shipments", new[] { "UserID" });
            DropIndex("dbo.Orders", new[] { "ShipmentID" });
            DropIndex("dbo.Orders", new[] { "CartID" });
            DropIndex("dbo.Languages", new[] { "LanguageName" });
            DropIndex("dbo.Genres", new[] { "GenreName" });
            DropIndex("dbo.WishLists", new[] { "BookID" });
            DropIndex("dbo.WishLists", new[] { "UserID" });
            DropIndex("dbo.User", new[] { "MailID" });
            DropIndex("dbo.User", new[] { "UserName" });
            DropIndex("dbo.User", new[] { "RoleID" });
            DropIndex("dbo.Carts", new[] { "UserID" });
            DropIndex("dbo.CartBooks", new[] { "BookID" });
            DropIndex("dbo.CartBooks", new[] { "CartID" });
            DropIndex("dbo.Book", new[] { "LanguageID" });
            DropIndex("dbo.Book", new[] { "GenreID" });
            DropIndex("dbo.Book", new[] { "Title" });
            DropIndex("dbo.Book", new[] { "UserID" });
            DropTable("dbo.Shipments");
            DropTable("dbo.Orders");
            DropTable("dbo.Languages");
            DropTable("dbo.Genres");
            DropTable("dbo.WishLists");
            DropTable("dbo.Roles");
            DropTable("dbo.User");
            DropTable("dbo.Carts");
            DropTable("dbo.CartBooks");
            DropTable("dbo.Book");
        }
    }
}
