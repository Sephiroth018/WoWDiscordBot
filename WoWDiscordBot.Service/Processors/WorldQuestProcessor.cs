using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MoreLinq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WoWClient;
using WoWDiscordBot.Service.Common;
using WoWDiscordBot.Service.Entities;
using WoWDiscordBot.Service.Entities.DAL;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace WoWDiscordBot.Service.Processors
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class WorldQuestProcessor : IProcessor
    {
        private readonly Func<RemoteWebDriver> _driverFactory;
        private readonly IApiClient _wowClient;

        public WorldQuestProcessor(Func<RemoteWebDriver> driverFactory,
                                   IApiClient wowClient)
        {
            _driverFactory = driverFactory;
            _wowClient = wowClient;
        }

        public ProcessorTypes ProcessorType => ProcessorTypes.WorldQuests;

        public void DoWork()
        {
            var watchedWorldQuestItems = new List<int>
                                         {
                                             133680
                                         };
            const string dayName = "day";
            const string hourName = "hour";
            const string minuteName = "minute";

            IEnumerable<(int itemId, int itemCount, int zoneId, DateTimeOffset availableUntil)> worldQuestData;
            using (var driver = _driverFactory.Invoke())
            {
                driver.Manage().Window.Size = new Size(1400, 800);

                var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                driver.Navigate().GoToUrl("http://www.wowhead.com/world-quests/eu");

                const string worldQuestRewardLinkSelector = "//a[contains(@class, 'icontiny')]";
                var worldQuestRows = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath($"{worldQuestRewardLinkSelector}/ancestor::tr")));

                worldQuestData = worldQuestRows.Select(row =>
                                                       {
                                                           var linkElement = row.FindElement(By.XPath($".{worldQuestRewardLinkSelector}"));
                                                           var itemId = ExtractIdFromLink(linkElement);
                                                           var itemNumberElement = row.FindElements(By.XPath($".{worldQuestRewardLinkSelector}/preceding-sibling::text()")).FirstOrDefault();
                                                           var itemCount = 1;
                                                           if (itemNumberElement != null)
                                                               itemCount = int.Parse(itemNumberElement.Text);

                                                           var zoneElement = row.FindElement(By.XPath(".//td[5]")); //TODO get name from zone master list by matching this id with location id
                                                           var zoneId = ExtractIdFromLink(zoneElement);
                                                           var availableUntilElement = row.FindElement(By.XPath(".//td[4]"));

                                                           var availableUntilData = Regex.Matches(availableUntilElement.Text, $"(?:(\\d+) ({dayName}|{hourName}|{minuteName}))", RegexOptions.IgnoreCase).Cast<Match>();
                                                           var hours = 0;
                                                           var minutes = 0;
                                                           var days = 0;

                                                           availableUntilData.ForEach(d =>
                                                                                      {
                                                                                          var groups = d.Groups.Cast<Group>().ToList();
                                                                                          var value = int.Parse(groups.First().Value);
                                                                                          var timePart = groups.Last().Value;
                                                                                          switch (timePart)
                                                                                          {
                                                                                              case dayName:
                                                                                                  days = value;
                                                                                                  break;
                                                                                              case hourName:
                                                                                                  hours = value;
                                                                                                  break;
                                                                                              case minuteName:
                                                                                                  minutes = value;
                                                                                                  break;
                                                                                              default:
                                                                                                  throw new ArgumentOutOfRangeException(nameof(timePart), timePart, "Unknown value");
                                                                                          }
                                                                                      });

                                                           var availableUntil = DateTimeOffset.Now.AddDays(days).AddHours(hours).AddMinutes(minutes);

                                                           return (itemId, itemCount, zoneId, availableUntil);
                                                       });

                driver.Quit();
            }

            //TODO get zone, make your own wow client...

            var worldQuests = worldQuestData.Where(data => watchedWorldQuestItems.Contains(data.itemId))
                                            .Select(wq =>
                                                    {
                                                        var item = RetryHelper.WithRetry(() => _wowClient.GetItem(wq.itemId), 3).Name;

                                                        return new ActiveWorldQuest
                                                               {
                                                                   ItemName = item,
                                                                   ItemCount = wq.itemCount,
                                                                   Zone = wq.zoneId,
                                                                   AvailableUntil = wq.availableUntil
                                                               };
                                                    });
        }

        public TimeSpan GetNextOccurence()
        {
            return (DateTime.Today.AddHours(DateTime.Now.Hour + 1) - DateTimeOffset.Now)
                .Add(TimeSpan.FromMinutes(1));
        }

        private static int ExtractIdFromLink(IWebElement linkElement)
        {
            var link = linkElement.GetAttribute("href");
            var itemId = int.Parse(link.Substring(link.IndexOf("=", StringComparison.Ordinal) + 1));
            return itemId;
        }
    }
}