using System.Drawing;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src.UI
{
    public class PlayerInfo(Font font, EventBus bus) : CanvaElement
    {
        [PlantUmlIgnoreAssociation]
        private readonly Font font = font;

        private readonly HpBar hpBar = new(bus: bus);

        private readonly Renderer renderer = new(
            new DrawImageCommand(
                texture: TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", column: 2, row: 2),
                transform: new(position: new(x: 1, y: 15), scale: new(width: 2, height: 2))
            )
        );

        public override void Draw()
        {
            renderer.Draw();
            hpBar.Draw();
            Engine.DrawText("Player", font, Brushes.White, new(x: 120, y: 590));
        }
    }
}
