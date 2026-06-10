using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.Drawing
{
    public class DrawTextCommand : IDrawCommand
    {
        public string text;
        public Font font;
        public Brush brush;
        public Point position;

        public void Draw(PaintEventArgs e)
        {
            e.Graphics.DrawString(text, font, brush, position);
        }
    }
}
