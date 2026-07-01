using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;
using EngineGDI.Src;

namespace SweeperRpg.Src
{
    public class LevelDataProps
    {
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color start { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color end { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color basic { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color lineMesh { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color treasure { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color background { get; set; }
    }

    public class LevelData
    {
        public LevelDataProps props { get; set; }
        public List<List<Cell>> grid { get; set; }

        public void Reset()
        {
            foreach (List<Cell> row in grid)
            {
                foreach (Cell cell in row)
                {
                    cell.Reset();
                }
            }
        }
    }
}
