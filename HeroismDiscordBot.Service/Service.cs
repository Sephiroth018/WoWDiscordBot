using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;
using HeroismDiscordBot.Service.Processors;
using HeroismDiscordBot.Service.Processors.Consumer;
using HeroismDiscordBot.Service.Processors.Producer;
using HeroismDiscordBot.Service.Processors.Transformer;
using WowDotNetAPI;

namespace HeroismDiscordBot.Service
{
    public partial class Service : ServiceBase
    {
        private DiscordSocketClient _discordClient;

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
            var configuration = new Configuration();
            _discordClient = new DiscordSocketClient();
            _discordClient.LoginAsync(TokenType.Bot, configuration.DiscordToken)
                          .Wait();
            _discordClient.StartAsync()
                          .Wait();
            _discordClient.GetConnectionsAsync()
                          .Wait();

            while (_discordClient.ConnectionState != ConnectionState.Connected)
                Task.Delay(500)
                    .Wait();

            var discordGuild = _discordClient.GetGuild(configuration.DiscordGuildId);

            //var channel = discordGuild.GetTextChannel(420312901157519362);
            //var messages = channel.GetMessagesAsync()
            //                      .ToList()
            //                      .Result.SelectMany(m => m.ToList());
            //channel.DeleteMessagesAsync(messages);
            var wowClient = new WowExplorer(Region.EU, Locale.de_DE, configuration.WoWApiKey);
            var classInfo = wowClient.GetCharacterClasses();
            var characterConsumer = new CharacterConsumer(discordGuild, configuration);
            
            using (var context = new BotContext())
            {
                // just to create the db
                context.Characters.Any();
                var characterProducer = new CharacterProducer(context, wowClient);
                var characterTransformer = new CharacterTransformer(wowClient, classInfo, context, configuration);
                var processor = new GuildMemberProcessor(characterTransformer, characterConsumer, characterProducer, configuration);
                processor.DoWork();
                context.SaveChanges();
            }
        }

        protected override void OnStop()
        {
            StopService();
        }

        private void StopService() { }
    }
}