using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src.SweeperRpg;

namespace EngineGDI.Src
{
    public class Defeat : Node
    {
        private Image defeatScreen = Image.FromFile("Assets/Imgs/defeat_placeholder.png");

        private bool toggle = false;

        public enum DefeatResult
        {
            None,
            Retry,
            MainMenu,
        }

        private int index = 0;
        private DefeatResult result = DefeatResult.None;

        private readonly Font font;

        public Defeat(Font font)
        {
            this.font = font;
        }

        public void EnableDefeat()
        {
            toggle = true;
            index = 0;
        }

        public void DisableDefeat()
        {
            toggle = false;
        }

        public override void Input()
        {
            if (!toggle)
                return;
            if (Engine.OnKeyDown(Keys.W))
                index = System.Math.Max(0, index - 1);
            if (Engine.OnKeyDown(Keys.S))
                index = (index + 1) % 2;
            if (Engine.OnKeyDown(Keys.Enter))
            {
                if (index == 0)
                    GameManager.Instance.OnPlay();
                else if (index == 1)
                    GameManager.Instance.OnMainMenu();
            }
        }

        public override void Draw()
        {
            if (!toggle)
                return;
            Engine.DrawImage(texture: defeatScreen, x: 0, y: 0);

            string retryText = (index == 0) ? "> Retry" : " Retry";
            string menuText = (index == 1) ? "> Main Menu" : " Main Menu";

            Engine.DrawText(retryText, font, Brushes.White, new PointF(400, 400));
            Engine.DrawText(menuText, font, Brushes.White, new PointF(400, 440));
        }
    }
}
