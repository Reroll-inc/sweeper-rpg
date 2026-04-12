using System.Collections.Generic;
using System.Drawing;

namespace EngineGDI.Src
{
    public class TileMap
    {
        public static readonly int size = 32;

        public static Bitmap LoadSprite(string path, int row, int column)
        {
            using (Bitmap sourceImage = new Bitmap(path))
            {
                Rectangle cropRect = new Rectangle(column * size, row * size, size, size);

                return sourceImage.Clone(cropRect, sourceImage.PixelFormat);
            }
        }
    }
}
