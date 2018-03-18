using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    public interface IRepository : IObjectContextAdapter, IDisposable
    {
        Database Database { get; }

        DbChangeTracker ChangeTracker { get; }

        DbContextConfiguration Configuration { get; }

        DbSet<GuildMembershipState> GuildMembershipHistory { get; set; }

        DbSet<GuildRank> GuildRankHistory { get; set; }

        DbSet<CharacterDiscordMessage> CharacterDiscordMessages { get; set; }

        DbSet<EventDiscordMessage> EventDiscordMessages { get; set; }

        DbSet<Character> Characters { get; set; }

        DbSet<Player> Players { get; set; }

        DbSet<Event> Events { get; set; }

        DbSet<Invitation> Invitations { get; set; }

        DbSet<Specialization> Specializations { get; set; }

        DbSet<MythicChallengeAffixData> MythicChallengeData { get; set; }

        DbSet<MythicChallengeAffix> Affixes { get; set; }

        DbSet<ProcessorState> ProcessorStates { get; set; }

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        DbSet Set(Type entityType);

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        DbEntityEntry Entry(object entity);

        string ToString();

        bool Equals(object obj);

        int GetHashCode();

        Type GetType();
    }
}