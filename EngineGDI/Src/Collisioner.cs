using System.Drawing;

namespace EngineGDI.Src
{
    public class Collisioner(Point position, Size size)
    {
        private Rectangle rect = new(
            location: new Point(
                x: position.X + (size.Width / 4),
                y: position.Y + (size.Height / 4)
            ),
            size: new Size(width: size.Width / 2, height: size.Height / 2)
        );

        public void UpdatePosition(Point position)
        {
            rect.X = position.X + (rect.Size.Width / 2);
            rect.Y = position.Y + (rect.Size.Height / 2);
        }

        public bool CheckCollision(Collisioner element)
        {
            return rect.X + rect.Width >= element.rect.X
                && rect.X <= element.rect.X + element.rect.Width
                && rect.Y + rect.Height >= element.rect.Y
                && rect.Y <= element.rect.Y + element.rect.Height;
        }
    }
}
