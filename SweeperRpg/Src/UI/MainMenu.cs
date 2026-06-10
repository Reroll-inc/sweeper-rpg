using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src.UI
{
    public class MainMenu(Font font) : ICanvaElement
    {
        private enum MenuResult
        {
            Play,
            Exit,
        }

        private readonly Font font = font;
        private readonly Image menuImg = Image.FromFile("Assets/Imgs/retro_main_menu.png");

        private MenuResult result = MenuResult.Play;

        public void Input()
        {
            if (Engine.OnKeyDown(Keys.W))
            {
                result = result == MenuResult.Play ? MenuResult.Exit : MenuResult.Play;
            }

            if (Engine.OnKeyDown(Keys.S))
            {
                result = result == MenuResult.Play ? MenuResult.Exit : MenuResult.Play;
            }

            if (Engine.OnKeyDown(Keys.Enter))
            {
                switch (result)
                {
                    case MenuResult.Play:
                        GameManager.Instance.OnPlay();
                        break;
                    case MenuResult.Exit:
                        GameManager.Instance.OnExit();
                        break;
                    default:
                        break;
                }
            }
        }

        public void Draw()
        {
            Engine.DrawImage(texture: menuImg, x: 0, y: 0);

            string playText = (result == MenuResult.Play) ? "> Play" : " Play";
            string exitText = (result == MenuResult.Exit) ? "> Exit" : " Exit";

            Engine.DrawText(playText, font, Brushes.White, new Point(200, 400));
            Engine.DrawText(exitText, font, Brushes.White, new Point(200, 450));
        }
    }
}
