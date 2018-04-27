using System;
using System.Globalization;
using Newtonsoft.Json;

namespace WoWClient.Converters
{
    public class DoubleConverter : JsonConverter<double?>
    {
        public override void WriteJson(JsonWriter writer, double? value, JsonSerializer serializer)
        {
            if (value.HasValue && (int)value.Value == value)
                writer.WriteRawValue(((int)value.Value).ToString());
            else if (value.HasValue)
                writer.WriteRawValue(value.Value.ToString(CultureInfo.InvariantCulture));
            else
                writer.WriteNull();
        }

        public override double? ReadJson(JsonReader reader, Type objectType, double? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            switch (reader.Value)
            {
                case long longValue:
                    return longValue;
                case double doubleValue:
                    return doubleValue;
                default:
                    return null;
            }
        }
    }
}