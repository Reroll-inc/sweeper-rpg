using System.Drawing;
using PlantUmlClassDiagramGenerator.Attributes;

namespace EngineGDI.Src
{
    public class TileMap
    {
        [PlantUmlIgnoreAssociation]
        public static int Size { get; } = 32;

        public static Bitmap LoadSprite(string path, int column, int row)
        {
            using Bitmap sourceImage = new(filename: path);
            Rectangle cropRect = new(x: column * Size, y: row * Size, width: Size, height: Size);

            return sourceImage.Clone(rect: cropRect, format: sourceImage.PixelFormat);
        }

        public static Bitmap LoadSprite(string path, int x, int y, Size size)
        {
            using Bitmap sourceImage = new(filename: path);

            Rectangle cropRect = new(x: x, y: y, width: size.Width, height: size.Height);

            return sourceImage.Clone(rect: cropRect, format: sourceImage.PixelFormat);
        }
    }
}
