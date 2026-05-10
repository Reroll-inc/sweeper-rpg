using System;
using System.Collections.Generic;
using System.Drawing;

namespace EngineGDI.Src.SweeperRpg
{
    /**
     * El tamaño de la grilla máxima tiene 16x32 cuadrantes de 32px.
     */
    public class Grid : Node
    {
        private readonly int MAX_ROW = 16;
        private readonly int MAX_COLUMN = 32;

        private LevelData level;
        private int lvlRow;
        private int lvlColumn;
        private int fillRows;
        private int fillColumns;

        public void SetLevel(LevelData level)
        {
            this.level = level;

            lvlRow = level.grid.Count;
            lvlColumn = level.grid[0].Count;

            if (lvlRow > MAX_ROW || lvlColumn > MAX_COLUMN)
            {
                throw new Exception(
                    $"Level size is [{lvlRow},{lvlColumn}] which is bigger than [16,32]"
                );
            }

            fillRows = (MAX_ROW - lvlRow) / 2;
            fillColumns = (MAX_COLUMN - lvlColumn) / 2;

            for (int rowId = 0; rowId < level.grid.Count; rowId++)
            {
                List<Cell> row = level.grid[rowId];

                for (int columnId = 0; columnId < row.Count; columnId++)
                {
                    Cell cell = row[columnId];

                    cell.SetData(
                        level: level,
                        columnId: columnId,
                        rowId: rowId,
                        fillColumns: fillColumns,
                        fillRows: fillRows
                    );
                }
            }
        }

        public void Reset()
        {
            foreach (List<Cell> row in level.grid)
            {
                foreach (Cell cell in row)
                {
                    cell.Reset();
                }
            }
        }

        public override void Update(float deltaTime)
        {
            //aca metemos el posible chequeo de colicion con el player
            foreach (List<Cell> row in level.grid)
            {
                foreach (Cell cell in row)
                {
                    cell.Update(deltaTime: deltaTime);
                }
            }
        }

        public override void Draw()
        {
            // Draw filling cells
            for (int rowId = 0; rowId < MAX_ROW; rowId++)
            {
                for (int columnId = 0; columnId < MAX_COLUMN; columnId++)
                {
                    Engine.DrawRect(
                        rect: new Rectangle(
                            location: new Point(x: columnId * Cell.SIZE, y: rowId * Cell.SIZE),
                            size: new Size(width: Cell.SIZE, height: Cell.SIZE)
                        ),
                        pen: new Pen(level.props.background),
                        brush: new SolidBrush(level.props.background)
                    );
                }
            }

            // Draw game cells
            foreach (List<Cell> row in level.grid)
            {
                foreach (Cell cell in row)
                {
                    cell.Draw();
                }
            }
        }

        public void DrawAfter()
        {
            foreach (List<Cell> row in level.grid)
            {
                foreach (Cell cell in row)
                {
                    cell.DrawAfter();
                }
            }
        }
    }
}
