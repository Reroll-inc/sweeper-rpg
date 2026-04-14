using System.Drawing;
using System.Numerics;

namespace EngineGDI.Src
{
    public class Collisioner : Node
    {
        private Rectangle rect;
        private readonly Pen pen;
        private readonly Brush brush;

        public Collisioner(Point position, Size size)
        {
            rect = new Rectangle(position, size);
            pen = new Pen(color: Color.Red);
            brush = new SolidBrush(color: Color.Blue);
        }

        public void UpdatePosition(Point position)
        {
            rect.X = position.X;
            rect.Y = position.Y;
        }

        public override void Draw()
        {
            Engine.DrawCollision(pen: pen, rect: rect);
        }
    }
}
