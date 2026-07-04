using System.Drawing;
using System.Windows.Forms;
using PlantUmlClassDiagramGenerator.Attributes;

namespace EngineGDI.Src.Drawing
{
    public class DrawTextCommand : IDrawCommand
    {
        public string text;

        [PlantUmlIgnoreAssociation]
        public Font font;

        [PlantUmlIgnoreAssociation]
        public Brush brush;

        [PlantUmlIgnoreAssociation]
        public Point position;

        public void Draw(PaintEventArgs e)
        {
            e.Graphics.DrawString(text, font, brush, position);
        }
    }
}
