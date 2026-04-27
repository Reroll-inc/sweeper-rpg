using System.Collections.Generic;
using System.Drawing;
using System.IO;
using EngineGDI.Src.SweeperRpg.UI;
using Newtonsoft.Json;

namespace EngineGDI.Src.SweeperRpg
{
    public class LevelManager : Node
    {
        // Quiero renderizar la grilla
        // Quiero crear los enemigos
        // Quiero crear el personaje
        private static readonly LevelManager instance = new LevelManager();
        private readonly Dictionary<int, List<List<Cell>>> levels =
            new Dictionary<int, List<List<Cell>>>();
        private int levelId;
        private List<List<Cell>> currentLevel;
        private readonly int MAX_ROW = 16;
        private readonly int MAX_COLUMN = 32;

        private int lvlRows;
        private int lvlColumns;
        private int fillRows;
        private int fillColumns;
        private bool created = false;
        private readonly Player player = new Player(x: 0, y: 0);
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
            currentLevel = JsonConvert.DeserializeObject<List<List<Cell>>>(jsonContent);

            levels.Add(level, currentLevel);
        }

        // TODO: Unload a level if changing level or going back to menu
        public void CreateLevel()
        {
            if (created)
                throw new System.Exception($"Level nº{levelId} already created");
            created = true;

            lvlRows = currentLevel.Count;
            lvlColumns = currentLevel[0].Count;

            if (lvlRows > 16 || lvlColumns > 32)
                throw new System.Exception(
                    $"Level size is [{lvlRows},{lvlColumns}] which is bigger than [16,32]"
                );

            fillRows = (MAX_ROW - lvlRows) / 2;
            fillColumns = (MAX_COLUMN - lvlColumns) / 2;

            for (int rowId = 0; rowId < lvlRows; rowId++)
            {
                List<Cell> row = currentLevel[rowId];

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
            LoadLevel(level);
            CreateLevel();
        }

        public void Init(Font font)
        {
            ui = new LevelUI(font: font, player: player);
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
