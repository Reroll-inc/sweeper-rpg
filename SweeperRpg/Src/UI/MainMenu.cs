using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src.UI
{
    public class MainMenu(Font font) : CanvaElement
    {
        [PlantUmlIgnore]
        private enum MenuResult
        {
            Play,
            Exit,
        }

        [PlantUmlIgnoreAssociation]
        private readonly Font font = font;
        private readonly Renderer renderer = new(
            new DrawImageCommand(
                texture: Image.FromFile("Assets/Imgs/retro_main_menu.png"),
                transform: new(position: new(x: 0, y: 0))
            )
        );

        [PlantUmlIgnoreAssociation]
        private MenuResult result = MenuResult.Play;

        public override void Input()
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

        public override void Draw()
        {
            renderer.Draw();

            string playText = (result == MenuResult.Play) ? "> Play" : " Play";
            string exitText = (result == MenuResult.Exit) ? "> Exit" : " Exit";

            Engine.DrawText(playText, font, Brushes.White, new Point(200, 400));
            Engine.DrawText(exitText, font, Brushes.White, new Point(200, 450));
        }
    }
}
