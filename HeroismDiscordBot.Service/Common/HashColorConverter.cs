using System;
using System.Drawing;
using Newtonsoft.Json;

namespace HeroismDiscordBot.Service.Common
{
    public class HashColorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(ColorTranslator.ToHtml(Color.FromArgb(((Color)value).ToArgb())));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ColorTranslator.FromHtml((string)reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }
    }
}