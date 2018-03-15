using System;
using Discord;
using HeroismDiscordBot.Service.Properties;
using WowDotNetAPI;

namespace HeroismDiscordBot.Service.Common
{
    public class Configuration : IConfiguration
    {
        public ulong DiscordGuildId => Settings.Default.DiscordGuildId;

        public string DiscordToken => Settings.Default.DiscordToken;

        public string WoWApiKey => Settings.Default.WoWApiKey;

        public string WoWRealm => Settings.Default.WoWRealm;

        public Region WoWRegion => (Region)Enum.Parse(typeof(Region), Settings.Default.WoWRegion);

        public string WoWGuild => Settings.Default.WoWGuild;

        public ulong DiscordMemberChangeChannelId => Settings.Default.DiscordMemberChangeChannelId;

        public Locale WoWLocale => (Locale)Enum.Parse(typeof(Locale), Settings.Default.WoWLocale);

        public ulong ErrorMessageTargetId => Settings.Default.ErrorMessageTargetId;

        public string DiscordCommandPrefix => Settings.Default.DiscordCommandPrefix;

        public Color DiscordBotMessageColor => Color.DarkTeal;

        public string WoWOAuthClientId => Settings.Default.WoWOAuthClientId;

        public string WoWOAuthClientSecret => Settings.Default.WoWOAuthClientSecret;

        public string WoWOAuthTokenEndpoint => Settings.Default.WoWOAuthTokenEndpoint;
    }
}