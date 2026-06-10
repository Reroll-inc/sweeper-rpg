using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using EngineGDI.Src;

namespace SweeperRpg.Src
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
        GOBLINBARBARIC,

        [JsonStringEnumMemberName("G_B1")]
        GOBLIN_BOSS_1,

        [JsonStringEnumMemberName("G_B2")]
        GOBLIN_BOSS_2,

        [JsonStringEnumMemberName("G_B3")]
        GOBLIN_BOSS_3,
    }

    public class Enemy : Node
    {
        private enum State
        {
            ALIVE,
            DEAD,
        }

        private static class DataManager
        {
            public static readonly Dictionary<EnemyKind, EnemyData> enemyData =
                JsonSerializer.Deserialize<Dictionary<EnemyKind, EnemyData>>(
                    File.ReadAllText("Assets/32rogues/monsters.json")
                );
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

        public int Damage { get; }
        private State state = State.ALIVE;

        public Collisioner Collisioner { get; }

        public Enemy(int x, int y, EnemyKind kind)
        {
            _ = DataManager.enemyData.TryGetValue(kind, out EnemyData data);

            Damage = data.damage;

            tile = TileMap.LoadSprite(
                path: "Assets/32rogues/monsters.png",
                row: data.point.X,
                column: data.point.Y
            );
            position = new Point(x: x, y: y);
            actualPosition = new Point(x: position.X * 32, y: position.Y * 32);

            Collisioner = new Collisioner(
                position: actualPosition,
                size: new Size(width: 32, height: 32)
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

            Collisioner.Reset(null);
        }

        public Enemy Clone(int x, int y)
        {
            Enemy clone = (Enemy)MemberwiseClone();

            clone.position = new Point(x: x, y: y);
            clone.actualPosition = new Point(x: clone.position.X * 32, y: clone.position.Y * 32);
            clone.Collisioner.UpdatePosition(clone.actualPosition);

            return clone;
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: tile, x: actualPosition.X, y: actualPosition.Y);
        }
    }
}
