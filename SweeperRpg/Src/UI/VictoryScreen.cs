using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src.UI
{
    public class VictoryScreen(Font font) : ICanvaElement
    {
        private enum Option
        {
            Next,
            MainMenu,
        }

        private readonly Image backgroundImg = Image.FromFile(
            "Assets/Imgs/victory_Placeholder.png"
        );
        private readonly Font font = font;

        private Option option = Option.Next;

        private readonly Renderer Renderer = new(
            new DrawImageCommand(
                texture: Image.FromFile("Assets/Imgs/victory_Placeholder.png"),
                transform: new(position: new(x: 0, y: 0))
            )
        );

        public void Input()
        {
            if (Engine.OnKeyDown(Keys.W))
            {
                option = option == Option.Next ? Option.MainMenu : Option.Next;
            }

            if (Engine.OnKeyDown(Keys.S))
            {
                option = option == Option.Next ? Option.MainMenu : Option.Next;
            }

            if (Engine.OnKeyDown(Keys.Enter))
            {
                switch (option)
                {
                    case Option.Next:
                        GameManager.Instance.NextLevel();
                        break;
                    case Option.MainMenu:
                        GameManager.Instance.OnMainMenu();
                        break;
                    default:
                        break;
                }
            }
        }

        public void Draw()
        {
            Renderer.Draw();

            string retryText = (option == Option.Next) ? "> Next level" : " Next Level";
            string menuText = (option == Option.MainMenu) ? "> Main Menu" : " Main Menu";

            Engine.DrawText(retryText, font, Brushes.White, new Point(400, 400));
            Engine.DrawText(menuText, font, Brushes.White, new Point(400, 440));
        }
    }
}
