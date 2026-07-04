using System.Drawing;
using System.Windows.Forms;
using PlantUmlClassDiagramGenerator.Attributes;

namespace EngineGDI.Src.Drawing
{
    public class DrawRectCommand : IDrawCommand
    {
        [PlantUmlIgnoreAssociation]
        public Rectangle rect;

        [PlantUmlIgnoreAssociation]
        public Pen pen;

        [PlantUmlIgnoreAssociation]
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
