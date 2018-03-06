using System.ServiceProcess;
using Discord;
using Discord.Rest;
using HeroismDiscordBot.Service.Entities;
using HeroismDiscordBot.Service.Processors;
using HeroismDiscordBot.Service.Properties;
using WowDotNetAPI;

namespace HeroismDiscordBot.Service
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartService();
        }

        public void StartService()
        {
            var discordClient = new DiscordRestClient();
            discordClient.LoginAsync(TokenType.Bot, Settings.Default.DiscordToken, false)
                         .Wait();
            var discordGuild = discordClient.GetGuildAsync(Settings.Default.DiscordGuildId)
                                            .Result;
            var wowClient = new WowExplorer(Region.EU, Locale.de_DE, Settings.Default.WoWApiKey);

            using (var database = new Database())
            {
                var processor = new GuildMemberProcessor(wowClient, database, discordGuild);
                processor.DoWork();
            }
        }

        protected override void OnStop()
        {
            StopService();
        }

        private void StopService()
        {

        }
    }
}