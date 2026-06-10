using System;
using System.Drawing;
using System.Text.Json.Serialization;
using EngineGDI.Src;
using SweeperRpg.Src.Animations;

namespace SweeperRpg.Src
{
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

    public class Cell : Node
    {
        private enum State
        {
            INIT = 'I',
            OPENING = 'O',
            OPEN = 'N',
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CellType type { get; set; } = CellType.NULL;
        public string id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnemyKind? kind { get; set; } = null;
        public int currency { get; set; }
        public static readonly int SIZE = 32;

        private Rectangle rect;
        public Rectangle Rect => rect;

        private State state = State.OPENING;
        private LevelData level;
        private readonly PeelingCellAnimation animation = new();

        public void SetData(LevelData level, int columnId, int rowId, int fillColumns, int fillRows)
        {
            this.level = level;

            Point point = new(
                x: (columnId * SIZE) + (fillColumns * SIZE),
                y: (rowId * SIZE) + (fillRows * SIZE)
            );
            Size size = new(SIZE, SIZE);

            rect = new Rectangle(location: point, size: size);

            animation.SetData(point: point, size: size);

            animation.OnFinish += FinishOpening;
        }

        public void StartOpening()
        {
            state = State.OPENING;
        }

        public void FinishOpening(object sender, EventArgs e)
        {
            state = State.OPEN;
        }

        public void Reset()
        {
            // TODO: This should reset to INIT
            state = State.OPENING;

            animation.Reset();
        }

        public override void Update(float deltaTime)
        {
            switch (state)
            {
                case State.OPENING:
                    animation.Update(deltaTime: deltaTime);
                    break;
                case State.INIT:
                    break;
                case State.OPEN:
                    break;
                default:
                    break;
            }
        }

        public override void Draw()
        {
            switch (type)
            {
                case CellType.COIN:
                    Engine.DrawRect(
                        rect: Rect,
                        pen: new Pen(level.props.lineMesh),
                        brush: new SolidBrush(level.props.treasure)
                    );
                    return;
                case CellType.START:
                    Engine.DrawRect(
                        rect: Rect,
                        pen: new Pen(level.props.lineMesh),
                        brush: new SolidBrush(level.props.start)
                    );
                    return;
                case CellType.END:
                    Engine.DrawRect(
                        rect: Rect,
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
                rect: Rect,
                pen: new Pen(level.props.lineMesh),
                brush: new SolidBrush(level.props.basic)
            );
        }

        public void DrawAfter()
        {
            switch (state)
            {
                case State.OPENING:
                    Engine.DrawACommand(animation);
                    break;
                case State.INIT:
                    break;
                case State.OPEN:
                    break;
                default:
                    break;
            }
        }
    }
}
