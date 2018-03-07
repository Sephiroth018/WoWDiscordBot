using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;
using MoreLinq;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = HeroismDiscordBot.Service.Entities.Character;

namespace HeroismDiscordBot.Service.Producer
{
    public class CharacterProducer : IProducer<(Region WoWRegion, string WoWRealm, string WoWGuild), IEnumerable<(GuildMember, Character)>>
    {
        private readonly Func<IRepository> _repositoryFactory;
        private readonly IWoWFactory _wowClient;

        public CharacterProducer(Func<IRepository> repositoryFactory, IWoWFactory wowClient)
        {
            _repositoryFactory = repositoryFactory;
            _wowClient = wowClient;
        }

        public IEnumerable<(GuildMember, Character)> GetData((Region WoWRegion, string WoWRealm, string WoWGuild) config)
        {
            var guild = _wowClient.GetClient().GetGuild(config.WoWRegion, config.WoWRealm, config.WoWGuild, GuildOptions.GetEverything);
            using (var repository = _repositoryFactory.Invoke())
            {
                return guild.Members
                            //.Take(10)
                            .FullJoin(repository.Characters.ToList(),
                                      m => m.Character.Name,
                                      c => c.Name,
                                      m => (m, (Character)null),
                                      c => ((GuildMember)null, c),
                                      (m, c) => (m, c));
            }
        }
    }
}