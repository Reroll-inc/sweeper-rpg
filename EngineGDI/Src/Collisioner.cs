using System.Drawing;

namespace EngineGDI.Src
{
    public class Collisioner : Node
    {
        private Rectangle rect;
        private readonly Pen pen;
        private readonly Brush brush;

        private bool debugCollisioned = false;

        public Collisioner(Point position, Size size, Color brushColor)
        {
            rect = new Rectangle(position, size);
            pen = new Pen(color: Color.Red);
            brush = new SolidBrush(color: brushColor);
        }

        public void UpdatePosition(Point position)
        {
            rect.X = position.X;
            rect.Y = position.Y;
        }

        public Rectangle Rect()
        {
            return rect;
        }

        public void OnCollisionIn(object s, CollisionEvent evt)
        {
            debugCollisioned = true;
        }

        public void OnCollisionOut(object s, CollisionEvent evt)
        {
            debugCollisioned = false;
        }

        public bool ChecCollision(Collisioner element)
        {
            return rect.X + rect.Width >= element.rect.X
                && rect.X <= element.rect.X + element.rect.Width
                && rect.Y + rect.Height >= element.rect.Y
                && rect.Y <= element.rect.Y + element.rect.Height;
        }

        public override void Draw()
        {
            Engine.DrawCollision(pen: pen, rect: rect, brush: debugCollisioned ? brush : null);
        }
    }
}
