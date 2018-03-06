using System.Data.Entity;
using HeroismDiscordBot.Service.Migrations;

namespace HeroismDiscordBot.Service.Entities
{
    public class BotContext : DbContext
    {
        public BotContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BotContext, Configuration>());
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<DiscordMessage> DiscordMessages { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
    }
}