using System;
using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src.UI
{
    public class VictoryScreen(Font font) : CanvaElement
    {
        [PlantUmlIgnore]
        private enum Option
        {
            Next,
            MainMenu,
        }

        [PlantUmlIgnoreAssociation]
        private readonly Font font = font;

        [PlantUmlIgnoreAssociation]
        private Option option = Option.Next;

        private bool isCompleted = false;

        private readonly Renderer renderer = new(
            new DrawImageCommand(
                texture: Image.FromFile("Assets/Imgs/victory_Placeholder.png"),
                transform: new(position: new(x: 0, y: 0))
            )
        );

        private readonly Renderer WinRenderer = new(
            new DrawImageCommand(
                texture: Image.FromFile("Assets/Imgs/you-win.png"),
                transform: new(
                    position: new(x: 0, y: 0),
                    scale: new(Program.SCREEN_HEIGHT / 1000f, Program.SCREEN_HEIGHT / 1000f),
                    offset: new(x: Program.SCREEN_HEIGHT / 5, y: 0)
                )
            )
        );
        private readonly Renderer WinBackgroundRenderer = new(
            new DrawRectCommand()
            {
                rect = new(0, 0, Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT),
                pen = new(color: Color.FromArgb(red: 25, green: 26, blue: 56)),
                brush = new SolidBrush(color: Color.FromArgb(red: 25, green: 26, blue: 56)),
            }
        );

        public void EnableVictory(bool isCompleted)
        {
            this.isCompleted = isCompleted;

            option = isCompleted ? Option.MainMenu : Option.Next;
        }

        public override void Input()
        {
            if (!isCompleted)
            {
                if (Engine.OnKeyDown(Keys.W))
                {
                    option = option == Option.Next ? Option.MainMenu : Option.Next;
                }

                if (Engine.OnKeyDown(Keys.S))
                {
                    option = option == Option.Next ? Option.MainMenu : Option.Next;
                }
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

        public override void Draw()
        {
            if (isCompleted)
            {
                WinBackgroundRenderer.Draw();
                WinRenderer.Draw();

                return;
            }
            renderer.Draw();

            string retryText = (option == Option.Next) ? "> Next level" : " Next Level";
            string menuText = (option == Option.MainMenu) ? "> Main Menu" : " Main Menu";

            Engine.DrawText(retryText, font, Brushes.White, new Point(400, 400));
            Engine.DrawText(menuText, font, Brushes.White, new Point(400, 440));
        }
    }
}
