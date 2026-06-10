using System.Windows.Forms;

namespace EngineGDI.Src.Drawing
{
    public interface IDrawCommand
    {
        public void Draw(PaintEventArgs e);
    }
}
