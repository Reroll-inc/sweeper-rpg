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

        [PlantUmlIgnoreAssociation]
        private Rectangle rect;
        private Transform transform;

        [PlantUmlIgnoreAssociation]
        private State state = State.CLOSED;
        private LevelData level;
        private readonly PeelingCellAnimation peelingAnimation = new();
        private int dmgAround;
        private int dmgAroundRelative;
        private Enemy enemy = null;
        private Renderer renderer;

        public void SetData(LevelData level, int columnId, int rowId, int fillColumns, int fillRows)
        {
            this.level = level;

            transform = new(position: new(x: columnId + fillColumns, y: rowId + fillRows));
            rect = new(location: transform.PositionAndScale, size: Transform.BaseUnit);

            Image tile = TileMap.LoadSprite(
                path: "Assets/32rogues/tiles.png",
                column: level.props.background.X,
                row: level.props.background.Y
            );
            renderer = new(
                new DrawImageCommand(
                    texture: tile,
                    transform: new(
                        position: new(x: columnId + fillColumns, y: rowId + fillRows),
                        scale: new(
                            (float)Transform.BaseUnit.Width / TileMap.Size,
                            (float)Transform.BaseUnit.Height / TileMap.Size
                        )
                    )
                )
            );

            peelingAnimation.SetData(transform: transform, color: level.props.foreground);

            peelingAnimation.OnFinish += FinishOpening;

            dmgAround = 0;
            dmgAroundRelative = 0;
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
            switch (type)
            {
                case CellType.COIN:
                    Engine.DrawRect(
                        rect: rect,
                        pen: new Pen(level.props.lineMesh),
                        brush: new SolidBrush(level.props.treasure)
                    );
                    return;
                case CellType.START:
                    Engine.DrawRect(
                        rect: rect,
                        pen: new Pen(level.props.lineMesh),
                        brush: new SolidBrush(level.props.start)
                    );
                    return;
                case CellType.END:
                    Engine.DrawRect(
                        rect: rect,
                        pen: new Pen(level.props.lineMesh),
                        brush: new SolidBrush(level.props.end)
                    );
                    return;
                case CellType.NULL:
                    break;
                case CellType.ENEMY:
                    break;
                default:
                    break;
            }

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
                    Engine.DrawRect(
                        rect: rect,
                        pen: new Pen(level.props.lineMesh),
                        brush: new SolidBrush(level.props.foreground)
                    );
                    break;
            }
        }
    }
}
