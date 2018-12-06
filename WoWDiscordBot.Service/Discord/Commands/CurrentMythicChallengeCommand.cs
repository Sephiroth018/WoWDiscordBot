using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using JetBrains.Annotations;
using WoWDiscordBot.Service.Discord.MessageHandlers;
using WoWDiscordBot.Service.Entities.DAL;

namespace WoWDiscordBot.Service.Discord.Commands
{
    [Name("CurrentMythicChallenge")]
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class CurrentMythicChallengeCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Func<IRepository> _repositoryFactory;
        private readonly IDiscordMessageBuilder<MythicChallengeAffixData> _messageBuilder;

        public CurrentMythicChallengeCommand(IDiscordMessageBuilder<MythicChallengeAffixData> messageBuilder, Func<IRepository> repositoryFactory)
        {
            _messageBuilder = messageBuilder;
            _repositoryFactory = repositoryFactory;
        }

        [Command("currentmythic")]
        [Summary("Gibt die aktuellen M+ Affixe aus.")]
        [UsedImplicitly(ImplicitUseKindFlags.Access)]
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