using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;
using EngineGDI.Src;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src
{
    [PlantUmlIgnore]
    public class LevelDataProps
    {
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color start { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color end { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color lineMesh { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color treasure { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color behind { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color background { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color foreground { get; set; }
    }

    public class LevelData
    {
        [PlantUmlIgnoreAssociation]
        public LevelDataProps props { get; set; }

        [PlantUmlIgnoreAssociation]
        public List<List<Cell>> grid { get; set; }

        public void Reset()
        {
            foreach (List<Cell> column in grid)
            {
                foreach (Cell cell in column)
                {
                    cell.Reset();
                }
            }
        }

        public void AnimateOnReset(Point position)
        {
            grid[position.Y][position.X].StartOpening();

            AnimateOnPlayerMove(position: position);
        }

        public void AnimateOnPlayerMove(Point position)
        {
            int left = position.X - 1;
            int right = position.X + 1;
            int top = position.Y - 1;
            int bottom = position.Y + 1;

            // 1. Left
            if (left >= 0)
            {
                grid[position.Y][left].StartOpening();

                // 2. Top left
                if (top >= 0)
                {
                    grid[top][left].StartOpening();
                }
            }

            // 3. Top
            if (top >= 0)
            {
                grid[top][position.X].StartOpening();

                // 4. Top right
                if (right < grid.Count)
                {
                    grid[top][right].StartOpening();
                }
            }

            // 5. Right
            if (right < grid.Count)
            {
                grid[position.Y][right].StartOpening();

                // 6. Bottom right
                if (bottom < grid[0].Count)
                {
                    grid[bottom][right].StartOpening();
                }
            }

            // 7. Bottom
            if (bottom < grid[0].Count)
            {
                grid[bottom][position.X].StartOpening();

                if (left >= 0)
                {
                    grid[bottom][left].StartOpening();
                }
            }
        }
    }
}
