using System.Drawing;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src.UI
{
    public class LevelUI(Font font, EventBus bus) : CanvaElement
    {
        private readonly PlayerInfo playerInfo = new(bus: bus);
        private readonly StageBar stageBar = new(font: font, bus: bus);

        public override void Draw()
        {
            playerInfo.Draw();
            stageBar.Draw();
        }
    }
}
