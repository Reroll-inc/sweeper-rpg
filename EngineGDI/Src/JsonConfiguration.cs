using System.Text.Json;

namespace EngineGDI.Src
{
    public static class JsonConfiguration
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            Converters = { new PointJsonConverter(), new ColorJsonConverter() },
        };
    }
}
