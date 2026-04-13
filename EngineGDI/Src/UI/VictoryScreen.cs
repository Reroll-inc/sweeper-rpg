using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace EngineGDI.Src.UI
{
    public class VictoryScreen : Node
    {
        private Image backgroundImg = Image.FromFile("Assets/Imgs/victory_Placeholder.png");
        private bool isActive = true;

        public VictoryScreen()
        {
            // Necesito que aparesca una pantalla de victoria.
            // Necesita botones.
        }

        public override void Input()
        {
            if (Engine.OnKeyDown(Keys.Enter))
            {
                isActive = false;
            }
        }

        public override void Draw()
        {
            if (isActive)
            {
                Engine.Draw(texture: backgroundImg, x: 0, y: 0);
            }
        }
    }
}
