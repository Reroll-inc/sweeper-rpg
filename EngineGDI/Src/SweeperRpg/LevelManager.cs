using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using static EngineGDI.Src.SweeperRpg.Grid;

namespace EngineGDI.Src.SweeperRpg
{
    public class LevelManager : Node
    {
        // Quiero renderizar la grilla
        // Quiero crear los enemigos
        // Quiero crear el personaje
        private static readonly LevelManager instance = new LevelManager();
        private readonly Dictionary<int, List<List<CellData>>> levels =
            new Dictionary<int, List<List<CellData>>>();
        private int levelId;
        private List<List<CellData>> currentLevel;
        private readonly int MAX_ROW = 16;
        private readonly int MAX_COLUMN = 32;

        private int lvlRow;
        private int lvlColumn;
        private int fillRows;
        private int fillColumns;
        private bool created = false;
        private readonly Player player = new Player(x: 0, y: 0);
        private static readonly List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> ActiveEnemies
        {
            get { return enemies; }
        }
        private readonly Dictionary<EnemyKind, Point> enemiesPoint;
        private static readonly CollisionManager collisionManager = CollisionManager.Instance;

        private LevelManager()
        {
            string jsonContent = File.ReadAllText("Assets/32rogues/monsters.json");

            enemiesPoint = JsonConvert.DeserializeObject<Dictionary<EnemyKind, Point>>(jsonContent);
        }

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
            currentLevel = JsonConvert.DeserializeObject<List<List<CellData>>>(jsonContent);

            levels.Add(level, currentLevel);
        }

        public void CreateLevel()
        {
            if (created)
                throw new System.Exception($"Level nº{levelId} already created");
            created = true;

            lvlRow = currentLevel.Count;
            lvlColumn = currentLevel[0].Count;

            if (lvlRow > 16 || lvlColumn > 32)
                throw new System.Exception(
                    $"Level size is [{lvlRow},{lvlColumn}] which is bigger than [16,32]"
                );

            fillRows = (MAX_ROW - lvlRow) / 2;
            fillColumns = (MAX_COLUMN - lvlColumn) / 2;

            for (int rowId = 0; rowId < lvlRow; rowId++)
            {
                List<CellData> row = currentLevel[rowId];

                for (int columnId = 0; columnId < row.Count; columnId++)
                {
                    CellData cell = row[columnId];

                    switch (cell.type)
                    {
                        case CellType.COIN:
                            //si la celda alguna moneda, los dibujamos :)
                            break;
                        case CellType.ENEMY:
                            if (!cell.kind.HasValue)
                                throw new System.Exception(
                                    $"Level nº{levelId}, cell [{rowId}, {columnId}] type is ENEMY but kind is not defined."
                                );
                            enemiesPoint.TryGetValue(cell.kind.Value, out Point inTile);

                            enemies.Add(
                                new Enemy(
                                    x: columnId + fillColumns,
                                    y: rowId + fillRows,
                                    inTile: inTile
                                )
                            );
                            break;
                        case CellType.START:
                            player.Reset(x: columnId + fillColumns, y: rowId + fillRows);
                            continue;
                        case CellType.END:
                            continue;
                        case CellType.NULL:
                            break;
                    }
                }
            }
        }

        public override void Input()
        {
            player.Input();
        }

        public override void Update(float deltaTime)
        {
            player.Update(deltaTime: deltaTime);
        }

        public override void Draw()
        {
            foreach (Node enemy in enemies)
                enemy.Draw();

            player.Draw();
        }

        public void OnCollision(Enemy enemy)
        {
            player.TakeDamage(enemy.Damage);
            if (player.IsDead())
            {
                // Avisar al GameManager que perdio.
            }
        }
    }

    // LevelManager should function as a sort of middle man that helps the combat loop work.
    // Player, al colisionar con un enemigo, deberia hacer que el LevelManager consiga el valor de danio del enemigo,
    // y le notifique a player el valor por el cual su vida debe bajar. Al mismo tiempo, por ahora, el Manager
    // deberia a la instancia de enemigo que colisiono cambiarle su estado para que muera.
}
