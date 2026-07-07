using System.Drawing;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src.UI
{
    public class PlayerInfo : CanvaElement
    {
        private int Hp = 0;

        [PlantUmlIgnoreAssociation]
        private readonly Font font;

        private readonly Renderer renderer = new(
            new DrawImageCommand(
                texture: TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", column: 2, row: 2),
                transform: new(position: new(x: 1, y: 19), scale: new(width: 2, height: 2))
            )
        );

        public PlayerInfo(Font font, EventBus bus)
        {
            this.font = font;

            bus.Subscribe<PlayerDmgEvent>(handler: HandlePlayerDmg);
            bus.Subscribe<PlayerResetEvent>(handler: HandlePlayerReset);
        }

        private void HandlePlayerDmg(PlayerDmgEvent data)
        {
            Hp = data.Hp - data.Dmg;
        }

        private void HandlePlayerReset(PlayerResetEvent data)
        {
            Hp = data.Hp;
        }

        public override void Draw()
        {
            renderer.Draw();
            Engine.DrawText("Player", font, Brushes.White, new(x: 120, y: 590));
            Engine.DrawText($"HP: {Hp}", font, Brushes.White, new(x: 120, y: 620));
        }
    }
}
