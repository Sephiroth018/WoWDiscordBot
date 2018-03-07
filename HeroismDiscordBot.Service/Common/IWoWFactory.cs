using System.Collections.Generic;
using WowDotNetAPI;
using WowDotNetAPI.Models;

namespace HeroismDiscordBot.Service.Common {
    public interface IWoWFactory {
        WowExplorer GetClient();

        IEnumerable<CharacterClassInfo> GetCharacterClasses();
    }
}