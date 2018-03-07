using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Consumer;
using HeroismDiscordBot.Service.Producer;
using HeroismDiscordBot.Service.Transformer;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = HeroismDiscordBot.Service.Entities.Character;

namespace HeroismDiscordBot.Service.Processors
{
    public class GuildMemberProcessor : IProcessor
    {
        private readonly ITransformer<(GuildMember, Character), (CharacterData, Character)> _characterDataTransformer;
        private readonly IProducer<(Region WoWRegion, string WoWRealm, string WoWGuild), IEnumerable<(GuildMember, Character)>> _characterProducer;
        private readonly ITransformer<(CharacterData, Character), Character> _characterTransformer;
        private readonly IConfiguration _configuration;
        private readonly IConsumer<Character, List<Character>> _discordMessageConsumer;

        public GuildMemberProcessor(ITransformer<(GuildMember, Character), (CharacterData, Character)> characterDataTransformer,
                                    IProducer<(Region WoWRegion, string WoWRealm, string WoWGuild), IEnumerable<(GuildMember, Character)>> characterProducer,
                                    IConfiguration configuration,
                                    ITransformer<(CharacterData, Character), Character> characterTransformer,
                                    IConsumer<Character, List<Character>> discordMessageConsumer)
        {
            _characterDataTransformer = characterDataTransformer;
            _characterProducer = characterProducer;
            _configuration = configuration;
            _characterTransformer = characterTransformer;
            _discordMessageConsumer = discordMessageConsumer;
        }

        public void DoWork()
        {
            var unboundedParallelism = new ExecutionDataflowBlockOptions
                                       {
                                           MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
                                       };
            var getDataBlock = new TransformManyBlock<(Region region, string realm, string guild), (GuildMember, Character)>(config => _characterProducer.GetData(config));
            var apiParallelism = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 10 };
            var smallApiParallelism = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 5 };
            var characterNormalizationBlock = new TransformBlock<(GuildMember, Character), (CharacterData, Character)>(data => _characterDataTransformer.Transform(data), apiParallelism);
            var characterBlock = new TransformBlock<(CharacterData, Character), Character>(data => _characterTransformer.Transform(data), unboundedParallelism);
            var discordMessageBlock = new TransformManyBlock<Character, Character>(data => _discordMessageConsumer.Consume(data), smallApiParallelism);
            var discordUpdateAltsMessageBlock = new ActionBlock<Character>(data => _discordMessageConsumer.Consume(data), smallApiParallelism);

            var propagateCompletionOption = new DataflowLinkOptions { PropagateCompletion = true };
            getDataBlock.LinkTo(characterNormalizationBlock, propagateCompletionOption);
            characterNormalizationBlock.LinkTo(characterBlock, propagateCompletionOption);
            characterBlock.LinkTo(discordMessageBlock, propagateCompletionOption);
            discordMessageBlock.LinkTo(discordUpdateAltsMessageBlock, propagateCompletionOption);

            getDataBlock.Post((_configuration.WoWRegion, _configuration.WoWRealm, _configuration.WoWGuild));
            getDataBlock.Complete();

            discordUpdateAltsMessageBlock.Completion.Wait();
        }
    }
}