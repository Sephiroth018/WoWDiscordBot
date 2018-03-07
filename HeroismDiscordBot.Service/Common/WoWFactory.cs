using System.Collections.Generic;
using WowDotNetAPI;
using WowDotNetAPI.Models;

namespace HeroismDiscordBot.Service.Common
{
    public class WoWFactory : IWoWFactory
    {
        private readonly IConfiguration _configuration;
        private WowExplorer _wowClient;

        public WoWFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public WowExplorer GetClient()
        {
            return _wowClient ?? (_wowClient = new WowExplorer(_configuration.WoWRegion, _configuration.WoWLocale, _configuration.WoWApiKey));
        }

        public IEnumerable<CharacterClassInfo> GetCharacterClasses()
        {
            return _wowClient.GetCharacterClasses();
        }
    }
}