using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src.UI
{
    public class DefeatScreen(Font font) : ICanvaElement
    {
        private readonly Image bgImg = Image.FromFile("Assets/Imgs/defeat_placeholder.png");

        private enum DefeatResult
        {
            Retry,
            MainMenu,
        }

        private DefeatResult result = DefeatResult.Retry;

        private readonly Font font = font;

        public void EnableDefeat()
        {
            result = DefeatResult.Retry;
        }

        public void Input()
        {
            if (Engine.OnKeyDown(Keys.W))
            {
                result = result == DefeatResult.Retry ? DefeatResult.MainMenu : DefeatResult.Retry;
            }

            if (Engine.OnKeyDown(Keys.S))
            {
                result = result == DefeatResult.Retry ? DefeatResult.MainMenu : DefeatResult.Retry;
            }

            if (Engine.OnKeyDown(Keys.Enter))
            {
                switch (result)
                {
                    case DefeatResult.Retry:
                        GameManager.Instance.OnRetry();
                        break;
                    case DefeatResult.MainMenu:
                        GameManager.Instance.OnMainMenu();
                        break;
                    default:
                        break;
                }
            }
        }

        public void Draw()
        {
            Engine.DrawImage(texture: bgImg, x: 0, y: 0);

            string retryText = (result == DefeatResult.Retry) ? "> Retry" : " Retry";
            string menuText = (result == DefeatResult.MainMenu) ? "> Main Menu" : " Main Menu";

            Engine.DrawText(retryText, font, Brushes.White, new Point(400, 400));
            Engine.DrawText(menuText, font, Brushes.White, new Point(400, 440));
        }
    }
}
