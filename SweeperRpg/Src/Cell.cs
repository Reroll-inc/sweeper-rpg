using System.Drawing;
using System.Text.Json.Serialization;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;
using SweeperRpg.Src.Animations;

namespace SweeperRpg.Src
{
    [PlantUmlIgnore]
    public enum CellType
    {
        [JsonStringEnumMemberName("N")]
        NULL,

        [JsonStringEnumMemberName("E")]
        ENEMY,

        [JsonStringEnumMemberName("C")]
        COIN,

        [JsonStringEnumMemberName("S")]
        START,

        [JsonStringEnumMemberName("D")]
        END,
    }

    public class Cell : IDynamicNode
    {
        [PlantUmlIgnoreAssociation]
        private static readonly Font dmgFont = new("Assets/Fonts/pixel.ttf", 12);

        [PlantUmlIgnore]
        private enum State
        {
            CLOSED = 'I',
            OPENING = 'O',
            OPEN = 'N',
        }

        [PlantUmlIgnoreAssociation]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CellType type { get; set; } = CellType.NULL;
        public string id { get; set; }

        [PlantUmlIgnoreAssociation]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnemyKind? kind { get; set; } = null;
        public int currency { get; set; }

        private Transform transform;

        [PlantUmlIgnoreAssociation]
        private State state = State.CLOSED;
        private LevelData level;
        private readonly PeelingCellAnimation peelingAnimation = new();
        private int dmgAround = 0;
        private int dmgAroundRelative = 0;
        private Enemy enemy = null;
        private Renderer renderer;

        public void Reload()
        {
            dmgAround = 0;

            Reset();
        }

        public void SetData(LevelData level, int columnId, int rowId, int fillColumns, int fillRows)
        {
            this.level = level;

            transform = new(
                position: new(x: columnId + fillColumns, y: rowId + fillRows),
                scale: new(
                    (float)Transform.BaseUnit.Width / TileMap.Size,
                    (float)Transform.BaseUnit.Height / TileMap.Size
                )
            );
            Point backgroundPoint = type switch
            {
                CellType.START => level.props.start,
                CellType.END => level.props.end,
                _ => level.props.background,
            };
            Image background = TileMap.LoadSprite(
                path: "Assets/32rogues/tiles.png",
                column: backgroundPoint.X,
                row: backgroundPoint.Y
            );
            renderer = new(new DrawImageCommand(texture: background, transform: transform));

            peelingAnimation.SetData(transform: transform, point: level.props.foreground);

            peelingAnimation.OnFinish += FinishOpening;
        }

        public void SetEnemy(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void AddDmgAround(int dmg)
        {
            dmgAround += dmg;
            dmgAroundRelative = dmgAround;
        }

        public void UpdateDmgAround(int dmg)
        {
            dmgAroundRelative -= dmg;
        }

        public void StartOpening()
        {
            if (state == State.CLOSED)
            {
                state = State.OPENING;
            }
        }

        public void FinishOpening()
        {
            state = State.OPEN;
        }

        public void Reset()
        {
            state = State.CLOSED;

            peelingAnimation.Reset();
            dmgAroundRelative = dmgAround;
        }

        public void Update(float deltaTime)
        {
            switch (state)
            {
                case State.OPENING:
                    peelingAnimation.Update(deltaTime: deltaTime);
                    break;
                case State.CLOSED:
                    break;
                case State.OPEN:
                    break;
                default:
                    break;
            }
        }

        public void Draw()
        {
            renderer.Draw();

            if ((enemy == null || !enemy.IsAlive()) && dmgAroundRelative > 0)
            {
                Engine.DrawText(
                    text: dmgAroundRelative.ToString(),
                    font: dmgFont,
                    brush: new SolidBrush(Color.Azure),
                    position: new(
                        transform.PositionAndScale.X + (dmgAroundRelative > 9 ? 8 : 16),
                        transform.PositionAndScale.Y + 12
                    )
                );
            }
        }

        public void DrawAfter()
        {
            switch (state)
            {
                case State.OPENING:
                    Engine.DrawACommand(peelingAnimation);
                    break;
                case State.CLOSED:
                    Engine.DrawACommand(peelingAnimation.BaseImageCmd);
                    break;
            }
        }
    }
}
