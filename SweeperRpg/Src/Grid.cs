using System;
using System.Collections.Generic;
using System.Drawing;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src
{
    public class Grid : IDynamicNode
    {
        public readonly int MAX_ROW = (int)
            Math.Ceiling((double)Program.SCREEN_HEIGHT / Transform.BaseUnit.Height);
        public readonly int MAX_COLUMN = (int)
            Math.Ceiling((double)Program.SCREEN_WIDTH / Transform.BaseUnit.Width);

        private readonly EnemyFactory enemyFactory;
        private readonly EventBus bus;
        private LevelData level;
        private int lvlRows;
        private int lvlColumns;
        private int fillRows;
        private int fillColumns;
        private readonly List<Renderer> emptyCells = [];

        public Grid(EventBus bus)
        {
            this.bus = bus;
            enemyFactory = new(bus: bus);

            bus.Subscribe<PlayerResetEvent>(handler: PlayerResetHandler);
            bus.Subscribe<PlayerMoveEvent>(handler: PlayerMoveHandler);
            bus.Subscribe<EnemyDeadEvent>(handler: EnemyDeadHandler);
        }

        private void PlayerResetHandler(PlayerResetEvent data)
        {
            level.AnimateOnReset(
                position: new(x: data.Position.X - fillColumns, y: data.Position.Y - fillRows)
            );
        }

        private void PlayerMoveHandler(PlayerMoveEvent data)
        {
            level.AnimateOnPlayerMove(
                position: new(x: data.Position.X - fillColumns, y: data.Position.Y - fillRows)
            );
        }

        private void EnemyDeadHandler(EnemyDeadEvent data)
        {
            Point position = data.Enemy.Transform.Position;

            level.UpdateDmgToCellsAround(
                dmg: data.Enemy.Damage,
                columnId: position.X - fillColumns,
                rowId: position.Y - fillRows
            );
        }

        public void GenerateLevel(LevelData level, List<Enemy> enemies, Player player)
        {
            this.level = level;

            lvlRows = level.grid.Count;
            lvlColumns = level.grid[0].Count;

            if (lvlRows > MAX_ROW || lvlColumns > MAX_COLUMN)
            {
                throw new Exception(
                    $"Level size is [{lvlRows},{lvlColumns}] which is bigger than [{MAX_ROW},{MAX_COLUMN}]"
                );
            }

            fillRows = (MAX_ROW - lvlRows) / 2;
            fillColumns = (MAX_COLUMN - lvlColumns) / 2;

            GenerateEmptyCells();

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
                            Enemy enemy = enemyFactory.Create(
                                x: columnId + fillColumns,
                                y: rowId + fillRows,
                                kind: cell.kind.Value
                            );
                            enemies.Add(item: enemy);

                            level.AddDmgToCellsAround(
                                dmg: enemy.Damage,
                                columnId: columnId,
                                rowId: rowId
                            );

                            cell.SetEnemy(enemy: enemy);

                            continue;
                        case CellType.START:
                            player.SetStart(x: columnId + fillColumns, y: rowId + fillRows);
                            continue;
                        case CellType.END:
                            continue;
                        case CellType.NULL:
                            break;
                    }
                }
            }
        }

        private void GenerateEmptyCells()
        {
            emptyCells.Clear();

            if (fillColumns == 0 && fillRows == 0)
            {
                return;
            }

            Random random = new();

            List<Image> backgrounds = level.props.behind.ConvertAll<Image>(
                (point) =>
                    TileMap.LoadSprite(
                        path: "Assets/32rogues/tiles.png",
                        column: point.X,
                        row: point.Y
                    )
            );

            for (int rowId = 0; rowId < MAX_ROW; rowId++)
            {
                for (int columnId = 0; columnId < MAX_COLUMN; columnId++)
                {
                    if (
                        rowId >= fillRows
                        && rowId < lvlRows + fillRows
                        && columnId >= fillColumns
                        && columnId < lvlColumns + fillColumns
                    )
                    {
                        continue;
                    }

                    emptyCells.Add(
                        new(
                            command: new DrawImageCommand(
                                texture: backgrounds[random.Next(backgrounds.Count)],
                                transform: new(
                                    position: new(x: columnId, y: rowId),
                                    scale: new(
                                        (float)Transform.BaseUnit.Width / TileMap.Size,
                                        (float)Transform.BaseUnit.Height / TileMap.Size
                                    )
                                )
                            )
                        )
                    );
                }
            }
        }

        public void Reset()
        {
            level.Reset();
        }

        public bool PlayerCanMove(Point position)
        {
            return position.X >= fillColumns
                && position.X <= (fillColumns + lvlColumns - 1)
                && position.Y >= fillRows
                && position.Y <= (fillRows + lvlRows - 1);
        }

        public void CheckIfPlayerWon(Player player)
        {
            CellType playerInCellType = level
                .grid[player.Transform.Position.Y - fillRows][
                    player.Transform.Position.X - fillColumns
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
            foreach (Renderer emptyCell in emptyCells)
            {
                emptyCell.Draw();
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
