using System;
using Newtonsoft.Json;

namespace WoWClient.Converters
{
    public class BoolBitConverter : JsonConverter<bool?>
    {
        public override void WriteJson(JsonWriter writer, bool? value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();
            else
                writer.WriteRawValue(value.Value ? "1" : "0");
        }

        public override bool? ReadJson(JsonReader reader, Type objectType, bool? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            switch (value)
            {
                case 0L:
                    return false;
                case 1L:
                    return true;
                case null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reader), value, "Unexpected value");
            }
        }
    }
}