using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EngineGDI.Src
{
    public class PointJsonConverter : JsonConverter<Point>
    {
        public override Point Read(
            ref Utf8JsonReader reader,
            System.Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            string value = reader.GetString();
            string[] parts = value.Split(',');
            return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public override void Write(
            Utf8JsonWriter writer,
            Point value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStringValue($"{value.X},{value.Y}");
        }
    }
}
