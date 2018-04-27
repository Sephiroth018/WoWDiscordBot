using System;
using System.Linq;
using JetBrains.Annotations;
using MoreLinq;
using WoWDiscordBot.Service.Discord.MessageHandlers;
using WoWDiscordBot.Service.Entities.DAL;
using WoWDiscordBot.Service.WoW;

namespace WoWDiscordBot.Service.Processors
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class MythicChallengeAffixesProcessor : IProcessor
    {
        private readonly IDiscordMessageSender<MythicChallengeAffixData> _messageSender;
        private readonly Func<IRepository> _repositoryFactory;
        private readonly IApiClient _wowClient;

        public MythicChallengeAffixesProcessor(IDiscordMessageSender<MythicChallengeAffixData> messageSender,
                                               Func<IRepository> repositoryFactory,
                                               IApiClient wowClient)
        {
            _messageSender = messageSender;
            _repositoryFactory = repositoryFactory;
            _wowClient = wowClient;
        }

        public ProcessorTypes ProcessorType => ProcessorTypes.MythicChallengeAffixes;

        public void DoWork()
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                var savedMythicData = repository.MythicChallengeData.OrderByDescending(m => m.Until).FirstOrDefault();

                if (savedMythicData?.Until > DateTimeOffset.Now)
                    return;

                var currentMythicData = _wowClient.GetMythicChallengeModeIndex().Result;

                if (savedMythicData == null || savedMythicData.From != currentMythicData.CurrentPeriodStartTimestamp)
                {
                    savedMythicData = repository.MythicChallengeData.Create();

                    savedMythicData.From = currentMythicData.CurrentPeriodStartTimestamp;
                    savedMythicData.Until = currentMythicData.CurrentPeriodEndTimestamp;
                    savedMythicData.Period = currentMythicData.CurrentPeriod;

                    currentMythicData.CurrentKeystoneAffixes
                                     .Select(a =>
                                             {
                                                 // ReSharper disable once AccessToDisposedClosure
                                                 var existingAffix = repository.Affixes.FirstOrDefault(ra => ra.StartingLevel == a.StartingLevel && ra.Affix == a.KeystoneAffix.Affix);

                                                 if (existingAffix == null)
                                                 {
                                                     // ReSharper disable once AccessToDisposedClosure
                                                     existingAffix = repository.Affixes.Create();
                                                     existingAffix.StartingLevel = a.StartingLevel;
                                                     existingAffix.Affix = a.KeystoneAffix.Affix;

                                                     // ReSharper disable once AccessToDisposedClosure
                                                     repository.Affixes.Add(existingAffix);
                                                 }

                                                 return existingAffix;
                                             })
                                     .ForEach(a => savedMythicData.Affixes.Add(a));

                    repository.MythicChallengeData.Add(savedMythicData);

                    repository.SaveChanges();

                    _messageSender.SendMessage(savedMythicData);
                }
            }
        }

        public TimeSpan GetNextOccurence()
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                var savedMythicData = repository.MythicChallengeData.OrderByDescending(m => m.Until).FirstOrDefault();
                var nextOccurence = (savedMythicData?.Until.AddMinutes(1) ?? DateTimeOffset.Now.AddMinutes(-1)) - DateTimeOffset.Now;
                var minOccurence = new TimeSpan(0, 0, 1);

                return nextOccurence < minOccurence ? minOccurence : nextOccurence;
            }
        }
    }
}