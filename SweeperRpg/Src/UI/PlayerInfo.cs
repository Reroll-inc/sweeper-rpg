using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src.UI
{
    public class PlayerInfo(EventBus bus) : CanvaElement
    {
        private readonly HpBar hpBar = new(bus: bus);

        public override void Draw()
        {
            hpBar.Draw();
        }
    }
}
