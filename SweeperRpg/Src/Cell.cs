using System.Drawing;
using System.Text.Json.Serialization;
using EngineGDI.Src;
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
        private int dmgAround = 0;
        private Enemy enemy = null;

        public void SetData(LevelData level, int columnId, int rowId, int fillColumns, int fillRows)
        {
            this.level = level;

            transform = new(position: new(x: columnId + fillColumns, y: rowId + fillRows));
            rect = new(location: transform.PositionAndScale, size: Transform.BaseUnit);

            peelingAnimation.SetData(transform: transform, color: level.props.foreground);

            peelingAnimation.OnFinish += FinishOpening;
        }

        public void SetEnemy(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void AddDmgAround(int dmg)
        {
            dmgAround += dmg;
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

            Engine.DrawRect(
                rect: rect,
                pen: new Pen(level.props.lineMesh),
                brush: new SolidBrush(level.props.background)
            );

            if ((enemy == null || !enemy.IsAlive()) && dmgAround > 0)
            {
                Engine.DrawText(
                    text: dmgAround.ToString(),
                    font: dmgFont,
                    brush: new SolidBrush(Color.Azure),
                    position: new(
                        transform.PositionAndScale.X + (dmgAround > 9 ? 8 : 16),
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
