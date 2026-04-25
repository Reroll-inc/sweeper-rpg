using System.Drawing;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EngineGDI.Src.SweeperRpg
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnemyKind
    {
        [EnumMember(Value = "G_R")]
        GOBLIN_ROGUE,

        [EnumMember(Value = "G_A")]
        GOBLIN_ARCHER,

        [EnumMember(Value = "G_M")]
        GOBLIN_MAGE,

        [EnumMember(Value = "G_C")]
        GOBLIN_BARBARIC,

        [EnumMember(Value = "G_B1")]
        GOBLIN_BOSS_1,

        [EnumMember(Value = "G_B2")]
        GOBLIN_BOSS_2,

        [EnumMember(Value = "G_B3")]
        GOBLIN_BOSS_3,
    }

    public class Enemy : Node
    {
        private Point position;
        private Point actualPosition;
        private readonly Image tile;
        private readonly Collisioner collisioner;
        private int damage = 2;

        public Collisioner Collisioner
        {
            get { return collisioner; }
        }

        public Enemy(int x, int y, Point inTile)
        {
            tile = TileMap.LoadSprite(
                path: "Assets/32rogues/monsters.png",
                row: inTile.X,
                column: inTile.Y
            );
            position = new Point(x: x, y: y);
            actualPosition = new Point(x: position.X * 32, y: position.Y * 32);

            collisioner = new Collisioner(
                position: actualPosition,
                size: new Size(width: 32, height: 32),
                brushColor: Color.BlanchedAlmond
            );
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: tile, x: actualPosition.X, y: actualPosition.Y);
        }

        public int Damage
        {
            get { return damage; }
        }
    }
}
