using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using HeroismDiscordBot.Service.Discord.MessageHandlers;
using HeroismDiscordBot.Service.Entities.DAL;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Discord.Commands
{
    [Name("CurrentMythicChallenge")]
    [UsedImplicitly]
    public class CurrentMythicChallengeCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Func<IRepository> _repositoryFactory;
        private readonly IDiscordMessageBuilder<MythicChallengeData> _messageBuilder;

        public CurrentMythicChallengeCommand(IDiscordMessageBuilder<MythicChallengeData> messageBuilder, Func<IRepository> repositoryFactory)
        {
            _messageBuilder = messageBuilder;
            _repositoryFactory = repositoryFactory;
        }

        [Command("currentmythic")]
        [Summary("Gibt die aktuellen M+ Affixe aus.")]
        [UsedImplicitly]
        public async Task CurrentMythic()
        {
            using (var unused = Context.Channel.EnterTypingState())
            {
                using (var repository = _repositoryFactory.Invoke())
                {
                    var savedMythicData = repository.MythicChallengeData.OrderByDescending(m => m.Until).FirstOrDefault();

                    if (savedMythicData == null)
                    {
                        await ReplyAsync("Daten wurden noch nicht geladen, bitte warten...");
                    }
                    
                    var message = _messageBuilder.BuildMessage(savedMythicData);

                    await ReplyAsync("", false, message);
                }
            }
        }
    }
}