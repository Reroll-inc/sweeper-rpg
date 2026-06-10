using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EngineGDI.Src.Drawing
{
    public class DrawImageCommand : IDrawCommand
    {
        public Image texture;
        public float X,
            Y,
            ScaleX,
            ScaleY;
        public float Angle,
            OffsetX,
            OffsetY;

        public void Draw(PaintEventArgs e)
        {
            float width = texture.Width * ScaleX;
            float height = texture.Height * ScaleY;

            InterpolationMode prevInterpolation = e.Graphics.InterpolationMode;
            e.Graphics.TranslateTransform(X, Y);
            e.Graphics.RotateTransform(Angle);

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(texture, -OffsetX * width, -OffsetY * height, width, height);
            e.Graphics.InterpolationMode = prevInterpolation;

            e.Graphics.ResetTransform();
        }
    }
}
