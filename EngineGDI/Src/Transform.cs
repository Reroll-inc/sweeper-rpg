using System.Drawing;

namespace EngineGDI.Src
{
    public class Transform(Point position, float rotation = 1f, Size? scale = null)
    {
        public Point Position = position;
        public float Rotation = rotation;
        public readonly Size BaseUnit = new(32, 32);
        public Size Scale = scale ?? new(1, 1);

        public Point PositionAndScale =>
            new(x: Position.X * BaseUnit.Width, y: Position.Y * BaseUnit.Height);
    }
}
