using System;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Discord;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;
using HeroismDiscordBot.Service.Processors;
using MoreLinq;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace HeroismDiscordBot.Service
{
    public partial class Service : ServiceBase
    {
        private readonly Container _container;

        public Service()
        {
            InitializeComponent();
            _container = new Container();
            BuildContainer();
        }

        protected override void OnStart(string[] args)
        {
            BuildContainer();
            StartService();
        }

        private void BuildContainer()
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            _container.Register<IConfiguration, Configuration>(Lifestyle.Singleton);
            _container.Register<IDiscordFactory, DiscordFactory>(Lifestyle.Singleton);
            _container.Register<IWoWFactory, WoWFactory>(Lifestyle.Scoped);
            _container.Register<IRepository, BotContext>(Lifestyle.Scoped);
            _container.RegisterSingleton<Func<IRepository>>(() => _container.GetInstance<IRepository>());
            _container.RegisterCollection<IProcessor>(new[] {Assembly.GetExecutingAssembly()});

            _container.Verify();
        }

        public void StartService()
        {
            //CleanUp();
            using (AsyncScopedLifestyle.BeginScope(_container))
            {
                var processors = _container.GetAllInstances<IProcessor>();

                processors.AsParallel().ForEach(p => p.DoWork());
            }
        }

        private void CleanUp()
        {
            using (AsyncScopedLifestyle.BeginScope(_container))
            {
                var configuration = _container.GetInstance<IConfiguration>();
                var discordGuild = _container.GetInstance<IDiscordFactory>().GetGuild();
                var channel = discordGuild.GetTextChannelAsync(configuration.DiscordMemberChangeChannelId).Result;
                var messages = channel.GetMessagesAsync().Flatten().Result.ToList();

                while (messages.Any())
                {
                    channel.DeleteMessagesAsync(messages.Select(m => m.Id)).Wait();
                    messages = channel.GetMessagesAsync().Flatten().Result.ToList();
                }

                using (var repository = _container.GetInstance<Func<IRepository>>().Invoke())
                {
                    repository.Players.RemoveRange(repository.Players);
                    repository.Events.RemoveRange(repository.Events);

                    repository.SaveChanges();
                }
            }
        }

        protected override void OnStop()
        {
            StopService();
        }

        private void StopService() { }
    }
}