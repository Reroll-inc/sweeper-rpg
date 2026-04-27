using System.Drawing;
using Newtonsoft.Json;

namespace EngineGDI.Src
{
    public class ColorHexConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(Color);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Color color = (Color)value;

            writer.WriteValue($"#{color.R:X2}{color.G:X2}{color.B:X2}");
        }

        public override object ReadJson(
            JsonReader reader,
            System.Type objectType,
            object existingValue,
            JsonSerializer serializer
        )
        {
            string hex = reader.Value.ToString();

            return ColorTranslator.FromHtml(hex);
        }
    }
}
