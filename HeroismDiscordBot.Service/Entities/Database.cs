using System.Data.Entity;

namespace HeroismDiscordBot.Service.Entities
{
    public class Database : DbContext
    {
        public Database()
            : base("DatabaseContext") { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
    }
}