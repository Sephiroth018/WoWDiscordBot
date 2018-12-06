﻿using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WoWDiscordBot.Service.Properties;

namespace WoWDiscordBot.Service.Common.Configuration
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Configuration : IConfiguration
    {
        public DiscordConfiguration Discord { get; set; } = new DiscordConfiguration();

        [JsonProperty("wow")]
        public WoWClientConfiguration WoW { get; set; } = new WoWClientConfiguration();

        public string Culture { get; set; }

        public static Configuration LoadConfiguration()
        {
            using (var stream = new FileInfo(Settings.Default.SettingsFile).OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    return JsonConvert.DeserializeObject<Configuration>(reader.ReadToEnd());
                }
            }
        }
    }
}