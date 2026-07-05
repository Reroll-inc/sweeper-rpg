using System.Drawing;
using PlantUmlClassDiagramGenerator.Attributes;

namespace EngineGDI.Src
{
    public class Transform(
        Point position,
        float rotation = 1f,
        SizeF? scale = null,
        Point? offset = null
    )
    {
        [PlantUmlIgnoreAssociation]
        public static readonly Size BaseUnit = new(42, 42);

        [PlantUmlIgnoreAssociation]
        public Point Position = position;
        public float Rotation = rotation;

        [PlantUmlIgnoreAssociation]
        public SizeF Scale = scale ?? new(1, 1);
        public Point Offset = offset ?? new(0, 0);

        [PlantUmlIgnoreAssociation]
        public Point PositionAndScale =>
            new(
                x: Position.X * BaseUnit.Width + Offset.X,
                y: Position.Y * BaseUnit.Height + Offset.Y
            );
    }
}
