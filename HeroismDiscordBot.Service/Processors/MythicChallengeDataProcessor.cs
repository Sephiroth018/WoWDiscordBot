using System;
using System.Linq;
using Flurl;
using Flurl.Http;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Discord.MessageHandlers;
using HeroismDiscordBot.Service.Entities.DAL;
using MoreLinq;

namespace HeroismDiscordBot.Service.Processors
{
    public class MythicChallengeDataProcessor : IProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly IDiscordMessageSender<MythicChallengeData> _messageSender;
        private readonly Func<IRepository> _repositoryFactory;

        public MythicChallengeDataProcessor(IDiscordMessageSender<MythicChallengeData> messageSender,
                                            IConfiguration configuration,
                                            Func<IRepository> repositoryFactory)
        {
            _messageSender = messageSender;
            _configuration = configuration;
            _repositoryFactory = repositoryFactory;
        }

        public void DoWork()
        {
            var tokenData = _configuration.WoWOAuthTokenEndpoint
                                          .SetQueryParam("grant_type", "client_credentials")
                                          .SetQueryParam("client_id", _configuration.WoWOAuthClientId)
                                          .SetQueryParam("client_secret", _configuration.WoWOAuthClientSecret)
                                          .GetJsonAsync()
                                          .Result;

            var currentMythicData = $"https://{_configuration.WoWRegion}.api.battle.net/data/wow/mythic-challenge-mode/?namespace=dynamic-{_configuration.WoWRegion}&locale=en_GB"
                                    .WithOAuthBearerToken((string)tokenData.access_token)
                                    .GetJsonAsync<Entities.MythicChallengeData>()
                                    .Result;

            using (var repository = _repositoryFactory.Invoke())
            {
                var savedMythicData = repository.MythicChallengeData.OrderByDescending(m => m.Until).FirstOrDefault();

                if (savedMythicData == null || savedMythicData.From != currentMythicData.From)
                {
                    savedMythicData = repository.MythicChallengeData.Create();

                    savedMythicData.From = currentMythicData.From;
                    savedMythicData.Until = currentMythicData.Until;
                    savedMythicData.Period = currentMythicData.CurrentPeriod;

                    currentMythicData.Affixes.Select(a => (StartingLevel: a.StartingLevel, Affix: (MythicChallengeAffixes)Enum.Parse(typeof(MythicChallengeAffixes), a.KeystoneAffix.Name)))
                                     .Select(a =>
                                             {
                                                 // ReSharper disable once AccessToDisposedClosure
                                                 var existingAffix = repository.Affixes.FirstOrDefault(ra => ra.StartingLevel == a.StartingLevel && ra.Affix == a.Affix);

                                                 if (existingAffix == null)
                                                 {
                                                     // ReSharper disable once AccessToDisposedClosure
                                                     existingAffix = repository.Affixes.Create();
                                                     existingAffix.StartingLevel = a.StartingLevel;
                                                     existingAffix.Affix = a.Affix;

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
                var nextOccurence = (savedMythicData?.Until ?? DateTimeOffset.Now.AddMinutes(-1)) - DateTimeOffset.Now;
                var minOccurence = new TimeSpan(0, 0, 1);

                return nextOccurence < minOccurence ? minOccurence : nextOccurence;
            }
        }
    }
}