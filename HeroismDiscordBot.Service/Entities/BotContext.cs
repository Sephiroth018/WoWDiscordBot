using System.Data.Entity;

namespace HeroismDiscordBot.Service.Entities
{
    public class BotContext : DbContext, IRepository
    {
        public DbSet<CharacterDiscordMessage> CharacterDiscordMessages { get; set; }

        public DbSet<EventDiscordMessage> EventDiscordMessages { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                        .HasMany(m => m.Characters)
                        .WithOptional(m => m.Player)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Character>()
                        .HasMany(m => m.Invitations)
                        .WithRequired(m => m.Character)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Character>()
                        .HasMany(m => m.DiscordMessages)
                        .WithOptional(m => m.Character)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Character>()
                        .HasMany(m => m.Invitations)
                        .WithRequired(m => m.Character)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Character>()
                        .HasMany(m => m.Specializations)
                        .WithRequired(m => m.Character)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Event>()
                        .HasMany(m => m.Invitations)
                        .WithRequired(m => m.Event)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Event>()
                        .HasRequired(m => m.DiscordMessage)
                        .WithOptional(m => m.Event)
                        .WillCascadeOnDelete();
        }
    }
}