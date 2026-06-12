using System.Drawing;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src.UI
{
    public class PlayerInfo(Font font, Player player) : ICanvaElement
    {
        private readonly Player player = player;
        private readonly Font font = font;

        private Renderer Renderer { get; } =
            new(
                new DrawImageCommand(
                    texture: player.Tile,
                    transform: new(position: new(x: 30, y: 600), scale: new(2, 2))
                )
            );

        public void Input() { }

        public void Draw()
        {
            Renderer.Draw();
            Engine.DrawText("Player", font, Brushes.White, new Point(120, 590));
            Engine.DrawText($"HP: {player.Hp}", font, Brushes.White, new Point(120, 620));
        }
    }
}
