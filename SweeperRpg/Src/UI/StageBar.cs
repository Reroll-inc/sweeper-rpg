using System.Drawing;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src.UI
{
    public class StageBar : CanvaElement
    {
        private int level = 0;

        private readonly Renderer textBoxRenderer;

        private readonly Transform transform;

        [PlantUmlIgnoreAssociation]
        private readonly Font font;

        public StageBar(Font font, EventBus bus)
        {
            this.font = font;

            bus.Subscribe<ChangeLevelEvent>(handler: HandleLevelChange);

            transform = new(
                position: new(x: 17, y: 1),
                scale: new(width: 5, height: 3),
                offset: new(x: 0, y: -Transform.BaseUnit.Height / 5)
            );
            textBoxRenderer = new(
                new DrawImageCommand(
                    texture: TileMap.LoadSprite(
                        path: "Assets/UI/text-box.png",
                        x: 0,
                        y: 0,
                        size: new(width: 52, height: 16)
                    ),
                    transform: transform
                )
            );
        }

        private void HandleLevelChange(ChangeLevelEvent data)
        {
            level = data.Level;
        }

        public override void Draw()
        {
            textBoxRenderer.Draw();

            Engine.DrawText(
                $"Stage: {level}",
                font,
                Brushes.White,
                new Point(
                    x: transform.PositionAndScale.X + (int)transform.Scale.Width * 3,
                    y: transform.PositionAndScale.Y + (int)transform.Scale.Height - 4
                )
            );
        }
    }
}
