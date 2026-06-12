using System.Drawing;

namespace EngineGDI.Src
{
    public class Transform(Point position, float rotation = 1f)
    {
        public Point Position = position;
        public float Rotation = rotation;
        public readonly Size Scale = new(32, 32);

        public Point PositionAndScale =>
            new(x: Position.X * Scale.Width, y: Position.Y * Scale.Height);
    }
}
