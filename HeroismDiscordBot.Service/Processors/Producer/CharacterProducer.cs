using System.Collections.Generic;
using HeroismDiscordBot.Service.Entities;
using MoreLinq;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = HeroismDiscordBot.Service.Entities.Character;

namespace HeroismDiscordBot.Service.Processors.Producer
{
    public class CharacterProducer
    {
        private readonly WowExplorer _wowClient;
        private readonly BotContext _botContext;

        public CharacterProducer(BotContext botContext, WowExplorer wowClient)
        {
            _botContext = botContext;
            _wowClient = wowClient;
        }

        public IEnumerable<(GuildMember, Character)> GetData((Region region, string realm, string guild) config)
        {
            var guild = _wowClient.GetGuild(config.region, config.realm, config.guild, GuildOptions.GetEverything);
            return guild.Members
                        //.Take(10)
                        .FullJoin(_botContext.Characters,
                                  m => m.Character.Name,
                                  c => c.Name,
                                  m => (m, (Character) null),
                                  c => ((GuildMember) null, c),
                                  (m, c) => (m, c));
        }
    }
}