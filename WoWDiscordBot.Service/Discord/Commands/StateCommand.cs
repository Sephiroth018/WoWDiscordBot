using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using JetBrains.Annotations;
using WoWDiscordBot.Service.Discord.MessageHandlers;
using WoWDiscordBot.Service.Entities.DAL;

namespace WoWDiscordBot.Service.Discord.Commands
{
    [Name("State")]
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class StateCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Func<IRepository> _repositoryFactory;
        private readonly IDiscordMessageBuilder<ProcessorState> _messageBuilder;

        public StateCommand(Func<IRepository> repositoryFactory, IDiscordMessageBuilder<ProcessorState> messageBuilder)
        {
            _repositoryFactory = repositoryFactory;
            _messageBuilder = messageBuilder;
        }

        [Command("state")]
        [Summary("Gibt den aktuellen Status der Verarbeitungstasks aus.")]
        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public async Task GetState()
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                var states = repository.ProcessorStates.ToList();

                await Task.WhenAll(states.Select(s => _messageBuilder.BuildMessage(s))
                                         .Select(m => ReplyAsync("", false, m)));
            }
        }
    }
}