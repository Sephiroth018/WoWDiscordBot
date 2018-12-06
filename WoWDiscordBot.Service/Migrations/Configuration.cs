using System.Data.Entity.Migrations;
using WoWDiscordBot.Service.Entities.DAL;

namespace WoWDiscordBot.Service.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BotContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BotContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}