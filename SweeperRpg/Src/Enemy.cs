using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src
{
    public class EnemyDeadEvent(Enemy enemy) : Event
    {
        public readonly Enemy Enemy = enemy;
    }

    [PlantUmlIgnore]
    public enum EnemyKind
    {
        [JsonStringEnumMemberName("G_R")]
        GOBLIN_ROGUE,

        [JsonStringEnumMemberName("G_A")]
        GOBLIN_ARCHER,

        [JsonStringEnumMemberName("G_M")]
        GOBLIN_MAGE,

        [JsonStringEnumMemberName("G_C")]
        GOBLINBARBARIC,

        [JsonStringEnumMemberName("G_B1")]
        GOBLIN_BOSS_1,

        [JsonStringEnumMemberName("G_B2")]
        GOBLIN_BOSS_2,

        [JsonStringEnumMemberName("G_B3")]
        GOBLIN_BOSS_3,
    }

    public class Enemy : IStaticNode
    {
        private static readonly Font dmgFont = new("Assets/Fonts/pixel.ttf", 10);

        [PlantUmlIgnore]
        private enum State
        {
            ALIVE,
            DEAD,
        }

        [PlantUmlIgnore]
        private static class DataManager
        {
            public static readonly Dictionary<EnemyKind, EnemyData> enemyData =
                JsonSerializer.Deserialize<Dictionary<EnemyKind, EnemyData>>(
                    File.ReadAllText("Assets/32rogues/monsters.json")
                );
        }

        [PlantUmlIgnore]
        private class EnemyData
        {
            [PlantUmlIgnoreAssociation]
            [JsonConverter(typeof(PointJsonConverter))]
            public Point point { get; set; }
            public int damage { get; set; }
        }

        public Transform Transform { get; private set; }

        [PlantUmlIgnoreAssociation]
        private readonly Image tile;

        public int Damage { get; }

        [PlantUmlIgnoreAssociation]
        private State state = State.ALIVE;

        public Collisioner Collisioner { get; private set; }
        private Renderer renderer;
        private readonly EventBus bus;

        public Enemy(int x, int y, EnemyKind kind, EventBus bus)
        {
            _ = DataManager.enemyData.TryGetValue(kind, out EnemyData data);
            this.bus = bus;

            Damage = data.damage;

            tile = TileMap.LoadSprite(
                path: "Assets/32rogues/monsters.png",
                column: data.point.Y,
                row: data.point.X
            );
            Transform = new(position: new(x: x, y: y), offset: new(4, -8));
            Collisioner = new Collisioner(transform: Transform);
            renderer = new(new DrawImageCommand(texture: tile, transform: Transform));
        }

        public void Defeat()
        {
            state = State.DEAD;
            bus.Publish<EnemyDeadEvent>(new(enemy: this));
        }

        public bool IsAlive()
        {
            return state == State.ALIVE;
        }

        public void Reset()
        {
            state = State.ALIVE;
        }

        public Enemy Clone(int x, int y)
        {
            Enemy clone = (Enemy)MemberwiseClone();

            // TODO: clonar el transform en vez de recrearlo
            clone.Transform = new(position: new(x: x, y: y), offset: new(4, -8));
            clone.Collisioner = new Collisioner(transform: clone.Transform);
            clone.renderer = new(new DrawImageCommand(texture: tile, transform: clone.Transform));

            return clone;
        }

        public void Draw()
        {
            renderer.Draw();
            Engine.DrawText(
                text: Damage.ToString(),
                font: dmgFont,
                brush: new SolidBrush(Color.Azure),
                position: new(
                    Transform.PositionAndScale.X + (Damage > 9 ? 6 : 12),
                    Transform.PositionAndScale.Y + TileMap.Size
                )
            );
        }
    }
}
