using System.Drawing;

namespace EngineGDI.Src
{
    public class Collisioner(Transform transform)
    {
        private Rectangle rect = new(
            location: new Point(
                x: transform.PositionAndScale.X + (transform.Scale.Width / 4),
                y: transform.PositionAndScale.Y + (transform.Scale.Height / 4)
            ),
            size: transform.Scale / 2
        );

        public void UpdatePosition(Transform transform)
        {
            rect.Size = transform.Scale / 2;
            rect.X = transform.PositionAndScale.X + (rect.Size.Width / 2);
            rect.Y = transform.PositionAndScale.Y + (rect.Size.Height / 2);
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
