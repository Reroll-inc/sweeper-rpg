using System.Drawing;
using EngineGDI.Src;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src.UI
{
    public class LevelUI(Font font, EventBus bus) : CanvaElement
    {
        private readonly PlayerInfo playerInfo = new(font: font, bus: bus);

        [PlantUmlIgnoreAssociation]
        private readonly Font font = font;
        private int level;

        public void SetLevel(int level)
        {
            this.level = level;
        }

        public override void Draw()
        {
            playerInfo.Draw();

            Engine.DrawText($"Game level: {level}", font, Brushes.White, new Point(120, 650));
        }
    }
}
