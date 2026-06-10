using System.Drawing;
using EngineGDI.Src;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src.UI
{
    public class LevelUI(Font font, Player player) : ICanvaElement
    {
        private readonly PlayerInfo playerInfo = new(font: font, player: player);
        private readonly Font font = font;
        private int level;

        public void SetLevel(int level)
        {
            this.level = level;
        }

        public void Input() { }

        public void Draw()
        {
            playerInfo.Draw();

            Engine.DrawText($"Game level: {level}", font, Brushes.White, new Point(120, 650));
        }
    }
}
