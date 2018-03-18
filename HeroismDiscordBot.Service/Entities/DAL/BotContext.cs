using System.Data.Entity;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    public class BotContext : DbContext, IRepository
    {
        public DbSet<MythicChallengeAffix> Affixes { get; set; }

        public DbSet<MythicChallengeData> MythicChallengeData { get; set; }

        public DbSet<GuildMembershipState> GuildMembershipHistory { get; set; }

        public DbSet<GuildRank> GuildRankHistory { get; set; }

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
                        .HasMany(m => m.Invitations)
                        .WithRequired(m => m.Character)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Character>()
                        .HasMany(m => m.Specializations)
                        .WithRequired(m => m.Character)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Character>()
                        .HasMany(m => m.GuildMembershipHistory)
                        .WithRequired(m => m.Character)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Character>()
                        .HasMany(m => m.GuildRankHistory)
                        .WithRequired(m => m.Character)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<GuildMembershipState>()
                        .HasOptional(m => m.DiscordMessage)
                        .WithRequired(m => m.GuildMembershipState)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Event>()
                        .HasMany(m => m.Invitations)
                        .WithRequired(m => m.Event)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Event>()
                        .HasOptional(m => m.DiscordMessage)
                        .WithRequired(m => m.Event)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<MythicChallengeData>()
                        .HasMany(m => m.Affixes)
                        .WithMany();
        }
    }
}