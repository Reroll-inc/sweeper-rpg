using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EngineGDI.Src
{
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color Read(
            ref Utf8JsonReader reader,
            System.Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            string hex = reader.GetString();
            return ColorTranslator.FromHtml(hex);
        }

        public override void Write(
            Utf8JsonWriter writer,
            Color value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
        }
    }
}
