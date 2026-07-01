using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using EngineGDI.Src;
using EngineGDI.Src.Nodes;
using SweeperRpg.Src.UI;

namespace SweeperRpg.Src
{
    public delegate void LevelEventLose();
    public delegate void LevelEventWin();

    public class LevelManager : InteractiveNode
    {
        public event LevelEventLose OnLose;
        public event LevelEventWin OnWin;

        private readonly Dictionary<int, LevelData> levels = [];

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
            grid.OnWin += CheckVictoryCondition;
        }

        private LevelData LoadLevel(int level)
        {
            if (levels.TryGetValue(key: level, out LevelData currentLevel))
            {
                currentLevel.Reset();

                return currentLevel;
            }

            string jsonContent = File.ReadAllText($"Assets/Levels/{level}.json");
            currentLevel = JsonSerializer.Deserialize<LevelData>(jsonContent);

            levels.Add(level, currentLevel);

            return currentLevel;
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
            if (grid.PlayerCanMove(position: position))
            {
                player.Move(position);
                grid.CheckIfPlayerWon(player: player);

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
            OnWin();
        }

        public void StartLevel(int level)
        {
            enemies.Clear();
            LevelData currentLevel = LoadLevel(level);

            grid.GenerateLevel(level: currentLevel, enemies: enemies, player: player);

            ui.SetLevel(level);
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
        }

        public override void Draw()
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
