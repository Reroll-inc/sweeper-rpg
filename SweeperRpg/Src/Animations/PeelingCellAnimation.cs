using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;

namespace SweeperRpg.Src.Animations
{
    public class PeelingCellAnimation : IDrawCommand
    {
        public event EventHandler OnFinish;
        private float progress;
        private readonly float sec = 1.5f;

        private Transform transform;

        public void SetData(Transform transform)
        {
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
                OnFinish?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Draw(PaintEventArgs e)
        {
            GraphicsState state = e.Graphics.Save();

            int withHeight = transform.BaseUnit.Width;
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

            e.Graphics.FillRectangle(
                new SolidBrush(Color.Green),
                new Rectangle(location: transform.PositionAndScale, size: transform.BaseUnit)
            );

            e.Graphics.Restore(state);
        }
    }
}
