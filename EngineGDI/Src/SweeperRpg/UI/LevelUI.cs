using System.Drawing;

namespace EngineGDI.Src.SweeperRpg.UI
{
    public class LevelUI : Node
    {
        private readonly PlayerInfo playerInfo;

        public LevelUI(Font font, Player player)
        {
            playerInfo = new PlayerInfo(font: font, player: player);
        }

        public override void Draw()
        {
            playerInfo.Draw();
        }
    }
}
