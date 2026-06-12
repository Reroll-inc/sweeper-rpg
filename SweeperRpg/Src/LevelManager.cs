using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using EngineGDI.Src;
using EngineGDI.Src.Nodes;
using SweeperRpg.Src.UI;

namespace SweeperRpg.Src
{
    public delegate void LevelEventLose();

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
    }

    public class LevelManager : IInteractiveNode
    {
        public event LevelEventLose OnLose;
        private readonly Dictionary<int, LevelData> levels = [];
        private LevelData currentLevel;
        private readonly int MAX_ROW = 16;
        private readonly int MAX_COLUMN = 32;

        private int lvlRows;
        private int lvlColumns;
        private int fillRows;
        private int fillColumns;

        public Renderer Renderer { get; }
        private readonly Player player = new(x: 0, y: 0);
        private static readonly List<Enemy> enemies = [];
        public static List<Enemy> ActiveEnemies => enemies.FindAll(static enemy => enemy.IsAlive());
        private static readonly Grid grid = new();

        private LevelUI ui;

        public static readonly LevelManager Instance = new();

        private LevelManager()
        {
            player.OnWillMove += PlayerWillMoveHandler;
            player.OnDeath += PlayerDeathHandler;
        }

        public void LoadLevel(int level)
        {
            if (levels.TryGetValue(key: level, out currentLevel))
            {
                return;
            }

            string jsonContent = File.ReadAllText($"Assets/Levels/{level}.json");
            currentLevel = JsonSerializer.Deserialize<LevelData>(jsonContent);

            levels.Add(level, currentLevel);
        }

        private void CreateLevel()
        {
            lvlRows = currentLevel.grid.Count;
            lvlColumns = currentLevel.grid[0].Count;

            if (lvlRows > 16 || lvlColumns > 32)
            {
                throw new System.Exception(
                    $"Level size is [{lvlRows},{lvlColumns}] which is bigger than [16,32]"
                );
            }

            fillRows = (MAX_ROW - lvlRows) / 2;
            fillColumns = (MAX_COLUMN - lvlColumns) / 2;

            for (int rowId = 0; rowId < lvlRows; rowId++)
            {
                List<Cell> row = currentLevel.grid[rowId];

                for (int columnId = 0; columnId < row.Count; columnId++)
                {
                    Cell cell = row[columnId];

                    switch (cell.type)
                    {
                        case CellType.COIN:
                            //si la celda alguna moneda, los dibujamos :)
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

        public void ResetLevel()
        {
            player.Reset();
            grid.Reset();

            foreach (Enemy enemy in enemies)
            {
                enemy.Reset();
            }
        }

        private void PlayerDeathHandler()
        {
            OnLose();
        }

        private void PlayerWillMoveHandler(Point position)
        {
            if (
                position.X >= fillColumns
                && position.X <= (fillColumns + lvlColumns - 1)
                && position.Y >= fillRows
                && position.Y <= (fillRows + lvlRows - 1)
            )
            {
                player.Move(position);

                RunCollisions();
                CheckVictoryCondition();
            }
        }

        private void RunCollisions()
        {
            foreach (Enemy enemy in ActiveEnemies)
            {
                if (player.TryCollide(enemy))
                {
                    return;
                }
            }
        }

        private void CheckVictoryCondition()
        {
            CellType playerInCellType = currentLevel
                .grid[player.Transform.Position.X - fillColumns][
                    player.Transform.Position.Y - fillRows
                ]
                .type;

            if (playerInCellType == CellType.END)
            {
                GameManager.Instance.OnVictory();
            }
        }

        public void StartLevel(int level)
        {
            enemies.Clear();
            LoadLevel(level);

            grid.SetLevel(currentLevel);

            CreateLevel();

            ui.SetLevel(level);
        }

        public void Init(Font font)
        {
            ui = new LevelUI(font: font, player: player);
        }

        public void Input()
        {
            player.Input();
        }

        public void Update(float deltaTime)
        {
            player.Update(deltaTime: deltaTime);
            grid.Update(deltaTime: deltaTime);
        }

        public void Draw()
        {
            grid.Draw();

            foreach (Enemy enemy in ActiveEnemies)
            {
                enemy.Draw();
            }

            player.Draw();

            grid.DrawAfter();

            ui.Draw();
        }
    }
}
