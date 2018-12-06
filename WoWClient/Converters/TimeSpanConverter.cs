using System;
using Newtonsoft.Json;

namespace WoWClient.Converters
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("time");
            writer.WriteValue(value.TotalMilliseconds);
            writer.WritePropertyName("hours");
            writer.WriteValue(value.TotalMilliseconds);
            writer.WritePropertyName("minutes");
            writer.WriteValue(value.TotalMilliseconds);
            writer.WritePropertyName("seconds");
            writer.WriteValue(value.TotalMilliseconds);
            writer.WritePropertyName("milliseconds");
            writer.WriteValue(value.TotalMilliseconds);
            writer.WritePropertyName("isPositive");
            writer.WriteValue(true);
            writer.WriteEndObject();
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return TimeSpan.FromMilliseconds(0);
        }
    }
}