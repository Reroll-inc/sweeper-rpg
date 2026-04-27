using System.Drawing;

namespace EngineGDI.Src.SweeperRpg.UI
{
    public class PlayerInfo : Node
    {
        private readonly Player player;

        private readonly Font font;

        public PlayerInfo(Font font, Player player)
        {
            this.player = player;
            this.font = font;
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: player.Tile, x: 30, y: 600, scaleX: 2, scaleY: 2);

            Engine.DrawText("Player", font, Brushes.White, new Point(120, 590));
            Engine.DrawText($"HP: {player.Hp}", font, Brushes.White, new Point(120, 620));
        }
    }
}
