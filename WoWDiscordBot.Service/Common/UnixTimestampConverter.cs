using System;
using System.Globalization;
using Newtonsoft.Json;

namespace WoWDiscordBot.Service.Common
{
    public class UnixTimestampConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTimeOffset)value - DateTimeOffset.FromUnixTimeSeconds(0)).TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return DateTimeOffset.FromUnixTimeMilliseconds((long)reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeOffset);
        }
    }
}