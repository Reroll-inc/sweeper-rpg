using System.Collections.Generic;
using System.Drawing;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src
{
    [PlantUmlIgnore]
    public class LevelDataProps
    {
        public Color start { get; set; }
        public Color end { get; set; }
        public Color lineMesh { get; set; }
        public Color treasure { get; set; }

        public List<Point> behind { get; set; }

        public Point background { get; set; }

        public Point foreground { get; set; }
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

        public void Reload()
        {
            foreach (List<Cell> column in grid)
            {
                foreach (Cell cell in column)
                {
                    cell.Reload();
                }
            }
        }

        public void AddDmgToCellsAround(int dmg, int columnId, int rowId)
        {
            int left = columnId - 1;
            int right = columnId + 1;
            int top = rowId - 1;
            int bottom = rowId + 1;

            // 1. Left
            if (left >= 0)
            {
                grid[rowId][left].AddDmgAround(dmg: dmg);

                // 2. Top left
                if (top >= 0)
                {
                    grid[top][left].AddDmgAround(dmg: dmg);
                }
            }

            // 3. Top
            if (top >= 0)
            {
                grid[top][columnId].AddDmgAround(dmg: dmg);

                // 4. Top right
                if (right < grid.Count)
                {
                    grid[top][right].AddDmgAround(dmg: dmg);
                }
            }

            // 5. Right
            if (right < grid.Count)
            {
                grid[rowId][right].AddDmgAround(dmg: dmg);

                // 6. Bottom right
                if (bottom < grid[0].Count)
                {
                    grid[bottom][right].AddDmgAround(dmg: dmg);
                }
            }

            // 7. Bottom
            if (bottom < grid[0].Count)
            {
                grid[bottom][columnId].AddDmgAround(dmg: dmg);

                if (left >= 0)
                {
                    grid[bottom][left].AddDmgAround(dmg: dmg);
                }
            }
        }

        public void UpdateDmgToCellsAround(int dmg, int columnId, int rowId)
        {
            int left = columnId - 1;
            int right = columnId + 1;
            int top = rowId - 1;
            int bottom = rowId + 1;

            // 1. Left
            if (left >= 0)
            {
                grid[rowId][left].UpdateDmgAround(dmg: dmg);

                // 2. Top left
                if (top >= 0)
                {
                    grid[top][left].UpdateDmgAround(dmg: dmg);
                }
            }

            // 3. Top
            if (top >= 0)
            {
                grid[top][columnId].UpdateDmgAround(dmg: dmg);

                // 4. Top right
                if (right < grid.Count)
                {
                    grid[top][right].UpdateDmgAround(dmg: dmg);
                }
            }

            // 5. Right
            if (right < grid.Count)
            {
                grid[rowId][right].UpdateDmgAround(dmg: dmg);

                // 6. Bottom right
                if (bottom < grid[0].Count)
                {
                    grid[bottom][right].UpdateDmgAround(dmg: dmg);
                }
            }

            // 7. Bottom
            if (bottom < grid[0].Count)
            {
                grid[bottom][columnId].UpdateDmgAround(dmg: dmg);

                if (left >= 0)
                {
                    grid[bottom][left].UpdateDmgAround(dmg: dmg);
                }
            }
        }

        public void AnimateOnReset(Point position)
        {
            grid[position.Y][position.X].StartOpening();

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

        public void AnimateOnPlayerMove(Point position)
        {
            grid[position.Y][position.X].StartOpening();

            // ¿Qué reglas debería haber al moverse en el tablero?
            // ¿"abro" la celda y luego avanzo?
            // ¿El final require que mate X enemigos?
        }
    }
}
