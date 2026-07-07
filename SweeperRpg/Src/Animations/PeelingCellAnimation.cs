using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;

namespace SweeperRpg.Src.Animations
{
    public delegate void PeelingEnd();

    public class PeelingCellAnimation : IDrawCommand
    {
        public event PeelingEnd OnFinish;
        private float progress;
        private readonly float sec = 0.8f;

        private Transform transform;
        public DrawImageCommand BaseImageCmd { get; private set; }

        public void SetData(Transform transform, Point point)
        {
            Image background = TileMap.LoadSprite(
                path: "Assets/32rogues/tiles.png",
                column: point.X,
                row: point.Y
            );
            BaseImageCmd = new DrawImageCommand(texture: background, transform: transform);

            this.transform = transform;
        }

        public void Reset()
        {
            progress = 0;
        }

        public void Update(float deltaTime)
        {
            progress += 100 * deltaTime / sec;

            if (progress > 100)
            {
                OnFinish();
            }
        }

        public void Draw(PaintEventArgs e)
        {
            GraphicsState state = e.Graphics.Save();

            int withHeight = Transform.BaseUnit.Width;
            int cutSize = (int)(withHeight * progress) / 50;

            GraphicsPath path = new();

            path.AddPolygon(
                points:
                [
                    new Point(
                        x: transform.PositionAndScale.X + cutSize,
                        y: transform.PositionAndScale.Y
                    ),
                    new Point(
                        x: transform.PositionAndScale.X + withHeight,
                        y: transform.PositionAndScale.Y
                    ),
                    new Point(
                        x: transform.PositionAndScale.X + withHeight,
                        y: transform.PositionAndScale.Y + withHeight
                    ),
                    new Point(
                        x: transform.PositionAndScale.X,
                        y: transform.PositionAndScale.Y + withHeight
                    ),
                    new Point(
                        x: transform.PositionAndScale.X,
                        y: transform.PositionAndScale.Y + cutSize
                    ),
                ]
            );

            path.CloseFigure();

            e.Graphics.SetClip(path);

            BaseImageCmd.Draw(e);

            e.Graphics.Restore(state);
        }
    }
}
