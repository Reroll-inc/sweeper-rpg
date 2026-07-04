using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src.UI
{
    public class DefeatScreen(Font font) : CanvaElement
    {
        private readonly Renderer renderer = new(
            new DrawImageCommand(
                texture: Image.FromFile("Assets/Imgs/defeat_placeholder.png"),
                transform: new(position: new(x: 0, y: 0))
            )
        );

        [PlantUmlIgnore]
        private enum DefeatResult
        {
            Retry,
            MainMenu,
        }

        [PlantUmlIgnoreAssociation]
        private DefeatResult result = DefeatResult.Retry;

        [PlantUmlIgnoreAssociation]
        private readonly Font font = font;

        public void EnableDefeat()
        {
            result = DefeatResult.Retry;
        }

        public override void Input()
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

        public override void Draw()
        {
            renderer.Draw();

            string retryText = (result == DefeatResult.Retry) ? "> Retry" : " Retry";
            string menuText = (result == DefeatResult.MainMenu) ? "> Main Menu" : " Main Menu";

            Engine.DrawText(retryText, font, Brushes.White, new Point(400, 400));
            Engine.DrawText(menuText, font, Brushes.White, new Point(400, 440));
        }
    }
}
