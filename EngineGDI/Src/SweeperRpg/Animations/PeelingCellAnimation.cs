using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static EngineGDI.Src.Engine;

namespace EngineGDI.Src.SweeperRpg.Animations
{
    public class PeelingCellAnimation : DrawCommand
    {
        public event EventHandler OnFinish;
        private float progress = 0;
        private readonly float sec = 1.5f;

        private Point point;
        private Size size;

        public void SetData(Point point, Size size)
        {
            this.point = point;
            this.size = size;
        }

        public void Reset()
        {
            progress = 0;
        }

        public void Update(float deltaTime)
        {
            progress += 100 * deltaTime / sec;

            if (progress > 100)
                OnFinish?.Invoke(this, EventArgs.Empty);
        }

        public override void Draw(PaintEventArgs e)
        {
            GraphicsState state = e.Graphics.Save();

            int withHeight = size.Width;
            int cutSize = (int)(withHeight * progress) / 50;

            GraphicsPath path = new GraphicsPath();

            path.AddPolygon(
                points: new Point[]
                {
                    new Point(x: point.X + cutSize, y: point.Y),
                    new Point(x: point.X + withHeight, y: point.Y),
                    new Point(x: point.X + withHeight, y: point.Y + withHeight),
                    new Point(x: point.X, y: point.Y + withHeight),
                    new Point(x: point.X, y: point.Y + cutSize),
                }
            );

            path.CloseFigure();

            e.Graphics.SetClip(path);

            e.Graphics.FillRectangle(
                new SolidBrush(Color.Green),
                new Rectangle(location: point, size: size)
            );

            e.Graphics.Restore(state);
        }
    }
}
