using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EngineGDI.Src.Drawing
{
    public class DrawImageCommand(Image texture, Transform transform) : IDrawCommand
    {
        public Image texture = texture;
        private readonly Transform transform = transform;

        public void Draw(PaintEventArgs e)
        {
            float width = texture.Width * transform.Scale.Width;
            float height = texture.Height * transform.Scale.Height;

            InterpolationMode prevInterpolation = e.Graphics.InterpolationMode;

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(
                texture,
                transform.PositionAndScale.X,
                transform.PositionAndScale.Y,
                width,
                height
            );
            e.Graphics.InterpolationMode = prevInterpolation;

            e.Graphics.ResetTransform();
        }
    }
}
