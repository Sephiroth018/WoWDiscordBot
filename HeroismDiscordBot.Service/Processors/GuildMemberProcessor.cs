using System.Threading.Tasks.Dataflow;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Processors.Consumer;
using HeroismDiscordBot.Service.Processors.Producer;
using HeroismDiscordBot.Service.Processors.Transformer;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = HeroismDiscordBot.Service.Entities.Character;

namespace HeroismDiscordBot.Service.Processors
{
    public class GuildMemberProcessor
    {
        private readonly CharacterTransformer _characterTransformer;
        private readonly CharacterConsumer _characterConsumer;
        private readonly CharacterProducer _characterProducer;
        private readonly Configuration _configuration;

        public GuildMemberProcessor(CharacterTransformer characterTransformer, CharacterConsumer characterConsumer, CharacterProducer characterProducer, Configuration configuration)
        {
            _characterTransformer = characterTransformer;
            _characterConsumer = characterConsumer;
            _characterProducer = characterProducer;
            _configuration = configuration;
        }

        public void DoWork()
        {
            var getDataBlock = new TransformManyBlock<(Region, string realm, string guild), (GuildMember, Character)>(config => _characterProducer.GetData(config));
            var characterNormalizationBlock = new TransformBlock<(GuildMember, Character), (CharacterData, Character)>(data => _characterTransformer.TransformToCharacterData(data));
            var joinedCharacterTransformerBlock = new TransformBlock<(CharacterData, Character), Character>(data => _characterTransformer.TransformToNewCharacter(data));
            var joinedCharacterActionBlock = new ActionBlock<Character>(c => _characterConsumer.ConsumeJoinedCharacter(c));
            var existingCharacterUpdateAction = new ActionBlock<(CharacterData, Character Character)>(data => _characterConsumer.ConsumeExistingCharacter(data));
            var leftCharacterActionBlock = new ActionBlock<(CharacterData, Character Character)>(data => _characterConsumer.ConsumeLeftCharacter(data.Character));

            getDataBlock.LinkTo(characterNormalizationBlock, new DataflowLinkOptions {PropagateCompletion = true});
            characterNormalizationBlock.LinkTo(joinedCharacterTransformerBlock, new DataflowLinkOptions { PropagateCompletion = true }, data => data.Item2 == null);
            characterNormalizationBlock.LinkTo(leftCharacterActionBlock, new DataflowLinkOptions { PropagateCompletion = true }, data => data.Item1 == null);
            characterNormalizationBlock.LinkTo(existingCharacterUpdateAction, new DataflowLinkOptions { PropagateCompletion = true });
            joinedCharacterTransformerBlock.LinkTo(joinedCharacterActionBlock, new DataflowLinkOptions { PropagateCompletion = true });

            getDataBlock.Post((_configuration.WoWRegion, _configuration.WoWRealm, _configuration.WoWGuild));
            getDataBlock.Complete();

            leftCharacterActionBlock.Completion.Wait();
            existingCharacterUpdateAction.Completion.Wait();
            joinedCharacterActionBlock.Completion.Wait();
        }
    }
}