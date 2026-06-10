using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.Drawing
{
    public class DrawRectCommand : IDrawCommand
    {
        public Rectangle rect;
        public Pen pen;
        public Brush brush;

        public void Draw(PaintEventArgs e)
        {
            if (brush is not null)
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            e.Graphics.DrawRectangle(pen, rect);
        }
    }
}
