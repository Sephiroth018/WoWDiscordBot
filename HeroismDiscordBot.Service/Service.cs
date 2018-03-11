using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Discord;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;
using HeroismDiscordBot.Service.Logging;
using HeroismDiscordBot.Service.Processors;
using MoreLinq;
using NLog;
using NLog.Targets;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using ILogger = HeroismDiscordBot.Service.Logging.ILogger;

namespace HeroismDiscordBot.Service
{
    public partial class Service : ServiceBase
    {
        private readonly Container _container;
        private Scope _scope;
        private IEnumerable<IProcessor> _processors;

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

            Target.Register<DiscordPrivateMessageTarget>("DiscordPrivateMessage");
            NLog.Config.ConfigurationItemFactory.Default.CreateInstance = type => _container.GetRegistration(type)?.GetInstance() ?? Activator.CreateInstance(type);

            _container.RegisterConditional(typeof(ILogger), context => typeof(NLogProxy<>).MakeGenericType(context.Consumer.ImplementationType), Lifestyle.Singleton, context => true);
            _container.Register<IConfiguration, Configuration>(Lifestyle.Singleton);
            _container.Register<IDiscordFactory, DiscordFactory>(Lifestyle.Singleton);
            _container.Register<IWoWFactory, WoWFactory>(Lifestyle.Scoped);
            _container.Register<IRepository, BotContext>(Lifestyle.Scoped);
            _container.RegisterSingleton<Func<IRepository>>(() => _container.GetInstance<IRepository>());
            _container.RegisterCollection<IProcessor>(new[] { Assembly.GetExecutingAssembly() });

            _container.Verify();
        }

        public void StartService()
        {
            //CleanUp();
            _scope = AsyncScopedLifestyle.BeginScope(_container);

            _processors = _container.GetAllInstances<IProcessor>()
                                    .ToList();

            _processors.ForEach(p => p.Start());
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

        private void StopService()
        {
            _processors.AsParallel().ForEach(p => p.Stop());

            _scope.Dispose();
            LogManager.Shutdown();
        }
    }
}