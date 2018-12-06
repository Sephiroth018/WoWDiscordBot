using System.Data.Entity.Migrations;

namespace WoWDiscordBot.Service.Migrations
{
    public partial class LastUpdateMigration : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Characters", "LastUpdate", "LastWoWUpdate");
        }

        public override void Down()
        {
            RenameColumn("dbo.Characters", "LastWoWUpdate", "LastUpdate");
        }
    }
}