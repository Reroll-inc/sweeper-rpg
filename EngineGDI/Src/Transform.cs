using System.Drawing;
using PlantUmlClassDiagramGenerator.Attributes;

namespace EngineGDI.Src
{
    public class Transform(Point position, float rotation = 1f, Size? scale = null)
    {
        [PlantUmlIgnoreAssociation]
        public static readonly Size BaseUnit = new(32, 32);

        [PlantUmlIgnoreAssociation]
        public Point Position = position;
        public float Rotation = rotation;

        [PlantUmlIgnoreAssociation]
        public Size Scale = scale ?? new(1, 1);

        [PlantUmlIgnoreAssociation]
        public Point PositionAndScale =>
            new(x: Position.X * BaseUnit.Width, y: Position.Y * BaseUnit.Height);
    }
}
