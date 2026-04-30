using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EngineGDI.Src.SweeperRpg
{
    public class Enemy : Node
    {
        public enum EnemyKind
        {
            [JsonStringEnumMemberName("G_R")]
            GOBLIN_ROGUE,

            [JsonStringEnumMemberName("G_A")]
            GOBLIN_ARCHER,

            [JsonStringEnumMemberName("G_M")]
            GOBLIN_MAGE,

            [JsonStringEnumMemberName("G_C")]
            GOBLIN_BARBARIC,

            [JsonStringEnumMemberName("G_B1")]
            GOBLIN_BOSS_1,

            [JsonStringEnumMemberName("G_B2")]
            GOBLIN_BOSS_2,

            [JsonStringEnumMemberName("G_B3")]
            GOBLIN_BOSS_3,
        }

        private enum State
        {
            ALIVE,
            DEAD,
        }

        private class EnemyData
        {
            [JsonConverter(typeof(PointJsonConverter))]
            public Point point { get; set; }
            public int damage { get; set; }
        }

        private Point position;
        private Point actualPosition;
        private readonly Image tile;
        private readonly Collisioner collisioner;
        private static readonly Dictionary<EnemyKind, EnemyData> enemyData =
            JsonSerializer.Deserialize<Dictionary<EnemyKind, EnemyData>>(
                File.ReadAllText("Assets/32rogues/monsters.json")
            );

        private readonly int damage;

        public int Damage
        {
            get { return damage; }
        }
        private State state = State.ALIVE;

        public Collisioner Collisioner
        {
            get { return collisioner; }
        }

        public Enemy(int x, int y, EnemyKind kind)
        {
            enemyData.TryGetValue(kind, out EnemyData data);

            damage = data.damage;

            tile = TileMap.LoadSprite(
                path: "Assets/32rogues/monsters.png",
                row: data.point.X,
                column: data.point.Y
            );
            position = new Point(x: x, y: y);
            actualPosition = new Point(x: position.X * 32, y: position.Y * 32);

            collisioner = new Collisioner(
                position: actualPosition,
                size: new Size(width: 32, height: 32),
                brushColor: Color.BlanchedAlmond
            );
        }

        public void Defeat()
        {
            state = State.DEAD;
        }

        public bool IsAlive()
        {
            return state == State.ALIVE;
        }

        public void Reset()
        {
            state = State.ALIVE;

            collisioner.Reset(null);
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: tile, x: actualPosition.X, y: actualPosition.Y);
        }
    }
}
