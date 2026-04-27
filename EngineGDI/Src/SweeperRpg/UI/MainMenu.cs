using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.SweeperRpg
{
    public class MainMenu : Node
    {
        public enum MenuResult
        {
            None,
            Play,
            Exit,
        }

        private readonly Font font;
        private Image menuImg;
        private Point position;
        private Point positionToUpdate = new Point();
        private int index = 0;

        private MenuResult result = MenuResult.None;

        public MainMenu(Font font)
        {
            this.font = font;

            menuImg = Image.FromFile("Assets/Imgs/menu_placeholder.png");
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: menuImg, x: 0, y: 0);

            string playText = (index == 0) ? "> Play" : " Play";
            string exitText = (index == 1) ? "> Exit" : " Exit";

            Engine.DrawText(playText, font, Brushes.White, new Point(200, 400));
            Engine.DrawText(exitText, font, Brushes.White, new Point(200, 450));
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
                    GameManager.Instance.OnPlay();
                else if (index == 1)
                    GameManager.Instance.OnExit();
            }
        }
    }
}
