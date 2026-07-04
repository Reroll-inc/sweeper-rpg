using System.Drawing;
using PlantUmlClassDiagramGenerator.Attributes;

namespace EngineGDI.Src
{
    public class TileMap
    {
        [PlantUmlIgnoreAssociation]
        public static int Size { get; } = 32;

        public static Bitmap LoadSprite(string path, int row, int column)
        {
            using Bitmap sourceImage = new(path);
            Rectangle cropRect = new(column * Size, row * Size, Size, Size);

            return sourceImage.Clone(cropRect, sourceImage.PixelFormat);
        }
    }
}
