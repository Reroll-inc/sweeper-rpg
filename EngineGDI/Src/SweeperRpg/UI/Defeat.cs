using System.Drawing;

namespace EngineGDI.Src
{
    public class Defeat : Node
    {
        public Image defeatScreen = Image.FromFile("Assets/Imgs/defeatPlaceHolder.png");

        public bool toggle = false;

        public void EnableDefeat()
        {
            toggle = true;
        }

        public void DisableDefeat()
        {
            toggle = false;
        }

        public override void Draw()
        {
            if (toggle)
                Engine.DrawImage(texture: defeatScreen, x: 0, y: 0);
        }
    }
}
