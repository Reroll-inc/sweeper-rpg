using System;
using System.Drawing;
using System.IO;
using PlantUmlClassDiagramGenerator.Attributes;

namespace EngineGDI.Src
{
    public class TileMap
    {
        [PlantUmlIgnoreAssociation]
        public static int Size { get; } = 32;

        private static Bitmap LoadBitmapSafely(string path)
        {
            byte[] imageBytes = File.ReadAllBytes(path);

            using MemoryStream ms = new(imageBytes);

            return new Bitmap(ms);
        }

        private static Bitmap CropImage(Bitmap source, Rectangle cropRect)
        {
            Bitmap result = new(cropRect.Width, cropRect.Height, source.PixelFormat);
            result.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(
                    source,
                    new Rectangle(0, 0, cropRect.Width, cropRect.Height),
                    cropRect,
                    GraphicsUnit.Pixel
                );
            }
            return result;
        }

        public static Bitmap LoadSprite(string path, int column, int row)
        {
            string fullPath = Path.IsPathRooted(path)
                ? path
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            using Bitmap sourceImage = LoadBitmapSafely(fullPath);
            Rectangle cropRect = new(column * Size, row * Size, Size, Size);
            return CropImage(sourceImage, cropRect);
        }

        public static Bitmap LoadSprite(string path, int x, int y, Size size)
        {
            string fullPath = Path.IsPathRooted(path)
                ? path
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            using Bitmap sourceImage = LoadBitmapSafely(fullPath);
            Rectangle cropRect = new(x, y, size.Width, size.Height);
            return CropImage(sourceImage, cropRect);
        }
    }
}
