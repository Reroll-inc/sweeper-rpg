using System;
using System.Drawing;
using System.Runtime.Serialization;
using EngineGDI.Src.SweeperRpg.Animations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static EngineGDI.Src.SweeperRpg.Enemy;

namespace EngineGDI.Src.SweeperRpg
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CellType
    {
        [EnumMember(Value = "N")]
        NULL,

        [EnumMember(Value = "E")]
        ENEMY,

        [EnumMember(Value = "C")]
        COIN,

        [EnumMember(Value = "S")]
        START,

        [EnumMember(Value = "D")]
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

        public CellType type = CellType.NULL;
        public string id = null;
        public EnemyKind? kind = null;
        public int currency = 0;
        public static readonly int SIZE = 32;

        private Rectangle rect;
        public Rectangle Rect
        {
            get { return rect; }
        }

        private State state = State.OPENING;
        private LevelData level;
        private readonly PeelingCellAnimation animation = new PeelingCellAnimation();

        public void SetData(LevelData level, int columnId, int rowId, int fillColumns, int fillRows)
        {
            this.level = level;

            Point point = new Point(
                x: (columnId * SIZE) + (fillColumns * SIZE),
                y: (rowId * SIZE) + (fillRows * SIZE)
            );
            Size size = new Size(SIZE, SIZE);

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
            // TODO> This should reset to INIT
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
            }
        }
    }
}
