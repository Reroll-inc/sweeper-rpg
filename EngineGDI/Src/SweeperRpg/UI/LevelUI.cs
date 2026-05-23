using System.Drawing;

namespace EngineGDI.Src.SweeperRpg.UI
{
    public class LevelUI(Font font, Player player) : Node
    {
        // private static readonly List<DrawCommand> drawQueue = [];

        private readonly PlayerInfo playerInfo = new(font: font, player: player);
        private readonly Font font = font;
        private int level;

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
