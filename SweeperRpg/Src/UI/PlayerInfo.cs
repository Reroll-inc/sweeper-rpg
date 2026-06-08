using System.Drawing;
using EngineGDI.Src;

namespace SweeperRpg.Src.UI
{
    public class PlayerInfo(Font font, Player player) : Node
    {
        private readonly Player player = player;
        private readonly Font font = font;

        public override void Draw()
        {
            Engine.DrawImage(texture: player.Tile, x: 30, y: 600, scaleX: 2, scaleY: 2);

            Engine.DrawText("Player", font, Brushes.White, new Point(120, 590));
            Engine.DrawText($"HP: {player.Hp}", font, Brushes.White, new Point(120, 620));
        }
    }
}
