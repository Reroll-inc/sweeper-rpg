using System.Drawing;

namespace EngineGDI.Src
{
    public class Collisioner(Point position, Size size, Color brushColor) : Node
    {
        private Rectangle rect = new(
            location: new Point(
                x: position.X + (size.Width / 4),
                y: position.Y + (size.Height / 4)
            ),
            size: new Size(width: size.Width / 2, height: size.Height / 2)
        );

        public Rectangle Rect => rect;

        private readonly Pen pen = new(color: Color.Red);
        private readonly Brush brush = new SolidBrush(color: brushColor);

        private bool debugCollisioned;

        public void UpdatePosition(Point position)
        {
            rect.X = position.X + (rect.Size.Width / 2);
            rect.Y = position.Y + (rect.Size.Height / 2);
        }

        public void Reset(Point? position)
        {
            if (position.HasValue)
            {
                rect.X = position.Value.X + (rect.Size.Width / 2);
                rect.Y = position.Value.Y + (rect.Size.Height / 2);
            }

            debugCollisioned = false;
        }

        public void OnCollisionIn()
        {
            debugCollisioned = true;
        }

        public void OnCollisionOut()
        {
            debugCollisioned = false;
        }

        public bool CheckCollision(Collisioner element)
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
