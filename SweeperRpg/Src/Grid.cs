using System;
using System.Collections.Generic;
using System.Drawing;
using EngineGDI.Src;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src
{
    public class Grid(EventBus bus) : IDynamicNode
    {
        public const int MAX_ROW = 16;
        public const int MAX_COLUMN = 32;

        private readonly EventBus bus = bus;
        private LevelData level;
        private int lvlRows;
        private int lvlColumns;
        private int fillRows;
        private int fillColumns;

        public void GenerateLevel(LevelData level, List<Enemy> enemies, Player player)
        {
            this.level = level;

            lvlRows = level.grid.Count;
            lvlColumns = level.grid[0].Count;

            if (lvlRows > MAX_ROW || lvlColumns > MAX_COLUMN)
            {
                throw new Exception(
                    $"Level size is [{lvlRows},{lvlColumns}] which is bigger than [16,32]"
                );
            }

            fillRows = (MAX_ROW - lvlRows) / 2;
            fillColumns = (MAX_COLUMN - lvlColumns) / 2;

            for (int rowId = 0; rowId < level.grid.Count; rowId++)
            {
                List<Cell> row = level.grid[rowId];

                for (int columnId = 0; columnId < row.Count; columnId++)
                {
                    Cell cell = row[columnId];

                    // 1. Configure cell
                    cell.SetData(
                        level: level,
                        columnId: columnId,
                        rowId: rowId,
                        fillColumns: fillColumns,
                        fillRows: fillRows
                    );

                    // 2. Configure what's in the cell
                    switch (cell.type)
                    {
                        case CellType.COIN:
                            break;
                        case CellType.ENEMY:
                            enemies.Add(
                                EnemyFactory.Create(
                                    x: columnId + fillColumns,
                                    y: rowId + fillRows,
                                    kind: cell.kind.Value
                                )
                            );
                            break;
                        case CellType.START:
                            player.SetStart(x: columnId + fillColumns, y: rowId + fillRows);
                            continue;
                        case CellType.END:
                            continue;
                        case CellType.NULL:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void Reset()
        {
            level.Reset();
        }

        public bool PlayerCanMove(Point position)
        {
            return (
                position.X >= fillColumns
                && position.X <= (fillColumns + lvlColumns - 1)
                && position.Y >= fillRows
                && position.Y <= (fillRows + lvlRows - 1)
            );
        }

        public void CheckIfPlayerWon(Player player)
        {
            CellType playerInCellType = level
                .grid[player.Transform.Position.X - fillColumns][
                    player.Transform.Position.Y - fillRows
                ]
                .type;

            if (playerInCellType == CellType.END)
            {
                bus.Publish<LevelWinEvent>(new());
            }
        }

        public void Update(float deltaTime)
        {
            foreach (List<Cell> row in level.grid)
            {
                foreach (Cell cell in row)
                {
                    cell.Update(deltaTime: deltaTime);
                }
            }
        }

        public void Draw()
        {
            for (int rowId = 0; rowId < MAX_ROW; rowId++)
            {
                for (int columnId = 0; columnId < MAX_COLUMN; columnId++)
                {
                    Engine.DrawRect(
                        rect: new Rectangle(
                            location: new Point(x: columnId * 32, y: rowId * 32),
                            size: new Size(width: 32, height: 32)
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
