using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.SweeperRpg
{
    public class MainMenu : Node
    {
        private Font font;
        private Image menuImg;
        private Point position;
        private Point positionToUpdate = new Point();
        private int index = 0;

        public enum MenuResult
        {
            None,
            Play,
            Exit,
        }

        private MenuResult result = MenuResult.None;

        public MainMenu()
        {
            font = new Font("Assets/Fonts/pixel.ttf", 16);
            menuImg = Image.FromFile("Assets/Imgs/menu_placeholder.png");
            //This should load resources only, do not use draw because it should not happen when the object is created.
            //Draw();
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: menuImg, x: 0, y: 0);

            string playText = (index == 0) ? "> Play" : " Play";
            string exitText = (index == 1) ? "> Exit" : " Exit";

            Engine.DrawText(playText, font, Brushes.White, new PointF(200, 400));
            Engine.DrawText(exitText, font, Brushes.White, new PointF(200, 450));
        }

        public override void Input()
        {
            if (Engine.OnKeyDown(Keys.W))
                index = System.Math.Max(0, index - 1);
            if (Engine.OnKeyDown(Keys.S))
                index = (index + 1) % 2;
            if (Engine.OnKeyDown(Keys.Enter))
            {
                if (index == 0)
                    result = MenuResult.Play;
                else if (index == 1)
                    result = MenuResult.Exit;
                //position.X = 1;
                //position.Y = 1;
            }
        }

        public MenuResult GetResult()
        {
            MenuResult temp = result;
            result = MenuResult.None;
            return temp;
        }
    }
}
