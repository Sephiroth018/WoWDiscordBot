namespace HeroismDiscordBot.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Joined = c.DateTime(nullable: false),
                        Class = c.String(),
                        Level = c.Int(nullable: false),
                        Left = c.DateTime(),
                        LastUpdate = c.DateTime(nullable: false),
                        Player_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "dbo.Specializations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ItemLevel = c.Int(nullable: false),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Start = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Character_Id = c.Int(),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Character_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Invitations", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Invitations", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Specializations", "Character_Id", "dbo.Characters");
            DropIndex("dbo.Invitations", new[] { "Event_Id" });
            DropIndex("dbo.Invitations", new[] { "Character_Id" });
            DropIndex("dbo.Specializations", new[] { "Character_Id" });
            DropIndex("dbo.Characters", new[] { "Player_Id" });
            DropTable("dbo.Players");
            DropTable("dbo.Invitations");
            DropTable("dbo.Events");
            DropTable("dbo.Specializations");
            DropTable("dbo.Characters");
        }
    }
}
