using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.SweeperRpg.UI
{
    public class VictoryScreen : Node
    {
        private enum Option
        {
            Next,
            MainMenu,
        }

        private readonly Image backgroundImg = Image.FromFile(
            "Assets/Imgs/victory_Placeholder.png"
        );
        private readonly Font font;

        private Option option = Option.Next;

        public VictoryScreen(Font font)
        {
            this.font = font;
        }

        public void EnableVictory()
        {
            option = Option.Next;
        }

        public override void Input()
        {
            if (Engine.OnKeyDown(Keys.W))
                option = option == Option.Next ? Option.MainMenu : Option.Next;
            if (Engine.OnKeyDown(Keys.S))
                option = option == Option.Next ? Option.MainMenu : Option.Next;

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
                }
            }
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: backgroundImg, x: 0, y: 0);

            string retryText = (option == Option.Next) ? "> Next level" : " Next Level";
            string menuText = (option == Option.MainMenu) ? "> Main Menu" : " Main Menu";

            Engine.DrawText(retryText, font, Brushes.White, new Point(400, 400));
            Engine.DrawText(menuText, font, Brushes.White, new Point(400, 440));
        }
    }
}
