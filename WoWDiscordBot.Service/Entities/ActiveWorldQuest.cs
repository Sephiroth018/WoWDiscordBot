using System;

namespace WoWDiscordBot.Service.Entities
{
    public class ActiveWorldQuest
    {
        public string ItemName { get; set; }

        public string Zone { get; set; }

        public int ItemCount { get; set; }

        public DateTimeOffset AvailableUntil { get; set; }
    }
}