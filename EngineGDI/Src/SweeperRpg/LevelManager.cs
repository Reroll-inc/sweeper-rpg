using System.Collections.Generic;
using System.Drawing;
using System.IO;
using EngineGDI.Src.SweeperRpg.UI;
using Newtonsoft.Json;

namespace EngineGDI.Src.SweeperRpg
{
    public class LevelDataProps
    {
        [JsonConverter(typeof(ColorHexConverter))]
        public Color start;
        public Color end;
        public Color basic;
        public Color lineMesh;
        public Color treasure;
        public Color background;
    }

    public class LevelData
    { // atributo props pubnlico que es de tipo LevelDataProps
        public LevelDataProps props;
        public List<List<Cell>> grid;
    }

    public class LevelManager : Node
    {
        // Quiero renderizar la grilla
        // Quiero crear los enemigos
        // Quiero crear el personaje
        private static readonly LevelManager instance = new LevelManager();
        private readonly Dictionary<int, LevelData> levels = new Dictionary<int, LevelData>();
        private int levelId;
        private LevelData currentLevel;
        private readonly int MAX_ROW = 16;
        private readonly int MAX_COLUMN = 32;

        private int lvlRows;
        private int lvlColumns;
        private int fillRows;
        private int fillColumns;
        private readonly Player player = new Player(x: 0, y: 0);
        public Player Player
        {
            get { return player; }
        }
        private static readonly List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> ActiveEnemies
        {
            get { return enemies.FindAll(enemy => enemy.IsAlive()); }
        }
        private static readonly Grid grid = new Grid();
        private static readonly CollisionManager collisionManager = CollisionManager.Instance;

        private LevelUI ui;

        private LevelManager() { }

        public static LevelManager Instance
        {
            get { return instance; }
        }

        public void LoadLevel(int level)
        {
            levelId = level;

            if (levels.TryGetValue(key: level, out currentLevel))
                return;

            string jsonContent = File.ReadAllText($"Assets/Levels/{level}.json");
            currentLevel = JsonConvert.DeserializeObject<LevelData>(jsonContent);

            levels.Add(level, currentLevel);
        }

        private void CreateLevel()
        {
            lvlRows = currentLevel.grid.Count;
            lvlColumns = currentLevel.grid[0].Count;

            if (lvlRows > 16 || lvlColumns > 32)
                throw new System.Exception(
                    $"Level size is [{lvlRows},{lvlColumns}] which is bigger than [16,32]"
                );

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
                                new Enemy(
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
                    }
                }
            }
        }

        public void ResetLevel()
        {
            player.Reset();
            grid.Reset();

            foreach (Enemy enemy in enemies)
                enemy.Reset();
        }

        public bool IsWithinLimits(Point position)
        {
            return position.X >= fillColumns
                && position.X <= (fillColumns + lvlColumns - 1)
                && position.Y >= fillRows
                && position.Y <= (fillRows + lvlRows - 1);
        }

        public void OnCollision(Enemy enemy)
        {
            player.TakeDamage(enemy.Damage);

            if (player.IsDead())
                // Avisar al GameManager que perdio.
                GameManager.Instance.OnDefeat();
            else
                enemy.Defeat();
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

        public void CheckVictoryCondition()
        {
            if (
                currentLevel
                    .grid[player.Position.X - fillColumns][player.Position.Y - fillRows]
                    .type == CellType.END
            )
                GameManager.Instance.OnVictory();
        }

        public override void Input()
        {
            player.Input();
        }

        public override void Update(float deltaTime)
        {
            player.Update(deltaTime: deltaTime);
            grid.Update(deltaTime: deltaTime);

            collisionManager.Update(deltaTime: deltaTime);
        }

        public override void Draw()
        {
            grid.Draw();

            foreach (Enemy enemy in ActiveEnemies)
                enemy.Draw();

            player.Draw();

            grid.DrawAfter();

            collisionManager.Draw();

            ui.Draw();
        }
    }
}
