using System.Drawing;

namespace EngineGDI.Src.SweeperRpg.UI
{
    public class LevelUI : Node
    {
        private readonly PlayerInfo playerInfo;
        private readonly Font font;
        private int level;

        public LevelUI(Font font, Player player)
        {
            this.font = font;
            playerInfo = new PlayerInfo(font: font, player: player);
        }

        public void SetLevel(int level)
        {
            this.level = level;
        }

        public override void Draw()
        {
            playerInfo.Draw();

            Engine.DrawText($"Level: {level}", font, Brushes.White, new Point(120, 650));
        }
    }
}
