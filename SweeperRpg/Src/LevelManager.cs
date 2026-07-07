using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EngineGDI.Src;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src
{
    public class LevelWinEvent : Event { }

    public class ChangeLevelEvent(int level) : Event
    {
        public readonly int Level = level;
    }

    public class LevelManager : InteractiveNode
    {
        [PlantUmlIgnoreAssociation]
        private readonly Dictionary<int, LevelData> levels = [];

        public Renderer Renderer { get; }
        private readonly Player player;

        [PlantUmlIgnoreAssociation]
        private static readonly List<Enemy> enemies = [];

        [PlantUmlIgnoreAssociation]
        public static List<Enemy> ActiveEnemies => enemies.FindAll(static enemy => enemy.IsAlive());
        private readonly Grid grid;
        private readonly EventBus bus;

        public LevelManager(EventBus bus)
        {
            this.bus = bus;

            player = new(bus: bus, x: 0, y: 0);
            grid = new(bus: bus);

            bus.Subscribe<PlayerWantToMoveEvent>(handler: PlayerWillMoveHandler);
        }

        private LevelData LoadLevel(int level)
        {
            if (levels.TryGetValue(key: level, value: out LevelData currentLevel))
            {
                currentLevel.Reload();

                return currentLevel;
            }

            string jsonContent = File.ReadAllText(path: $"Assets/Levels/{level}.json");
            currentLevel = JsonSerializer.Deserialize<LevelData>(json: jsonContent);

            levels.Add(key: level, value: currentLevel);

            return currentLevel;
        }

        public void ResetLevel()
        {
            grid.Reset();
            player.Reset();

            foreach (Enemy enemy in enemies)
            {
                enemy.Reset();
            }
        }

        private void PlayerWillMoveHandler(PlayerWantToMoveEvent data)
        {
            if (grid.PlayerCanMove(position: data.Position))
            {
                player.Move(newPosition: data.Position);
                grid.CheckIfPlayerWon(player: player);

                RunCollisions();
            }
        }

        private void RunCollisions()
        {
            foreach (Enemy enemy in ActiveEnemies)
            {
                if (player.TryCollide(enemy: enemy))
                {
                    return;
                }
            }
        }

        public void StartLevel(int level)
        {
            enemies.Clear();
            LevelData currentLevel = LoadLevel(level: level);

            grid.GenerateLevel(level: currentLevel, enemies: enemies, player: player);

            bus.Publish<ChangeLevelEvent>(new(level: level));
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
        }
    }
}
