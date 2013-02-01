namespace QuickStartProject.Data.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        Password = c.String(),
                        Salt = c.String(),
                        Name = c.String(),
                        Company = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        To = c.String(),
                        From = c.String(),
                        Subject = c.String(),
                        Cc = c.String(),
                        Body = c.String(),
                        IsHtml = c.Boolean(nullable: false),
                        IsSent = c.Boolean(nullable: false),
                        SendAttempt = c.Int(nullable: false),
                        IsForceSend = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                        SubmitTime = c.DateTime(nullable: false),
                        SendTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.Binary(),
                        ContentType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Images");
            DropTable("dbo.Emails");
            DropTable("dbo.Users");
        }
    }
}
