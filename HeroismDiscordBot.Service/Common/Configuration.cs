using System;
using HeroismDiscordBot.Service.Properties;
using WowDotNetAPI;

namespace HeroismDiscordBot.Service.Common
{
    public class Configuration
    {
        public ulong DiscordGuildId => Settings.Default.DiscordGuildId;
        public string DiscordToken => Settings.Default.DiscordToken;
        public string WoWApiKey => Settings.Default.WoWApiKey;
        public string WoWRealm => Settings.Default.WoWRealm;
        public Region WoWRegion => (Region)Enum.Parse(typeof(Region), Settings.Default.WoWRegion);
        public string WoWGuild => Settings.Default.WoWGuild;
        public ulong DiscordMemberChangeChannelId => Settings.Default.DiscordMemberChangeChannelId;
    }
}