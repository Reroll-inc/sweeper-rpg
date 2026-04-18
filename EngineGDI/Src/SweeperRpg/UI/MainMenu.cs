using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.sweeperRpg
{
    public class MainMenu : Node
    {
        private Font font;
        private Image menuImg;
        private Point position;
        private Point positionToUpdate = new Point();
        private int index = 0;

        public MainMenu()
        {
            font = new Font("Assets/Fonts/pixel.ttf", 16);
            menuImg = Image.FromFile("Assets/Imgs/menu_placeholder.png");
            Draw();
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: menuImg, x: 0, y: 0);
        }

        public override void Input()
        {
            if (Engine.OnKeyDown(Keys.W))
                index = System.Math.Max(0, index - 1);
            if (Engine.OnKeyDown(Keys.S))
                index = (index + 1) % 2;
            if (Engine.OnKeyDown(Keys.Enter))
            {
                //position.X = 1;
                //position.Y = 1;
            }
        }
    }
}
