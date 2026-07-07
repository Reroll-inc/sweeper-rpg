using System.Drawing;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src.UI
{
    public class HpBar : CanvaElement
    {
        private int BaseHp = 0;
        private int Hp = 0;
        private static Size Scale = new(width: 4, height: 4);

        private readonly Renderer HpFrameRenderer = new(
            new DrawImageCommand(
                texture: TileMap.LoadSprite(
                    path: "Assets/UI/player-bars.png",
                    x: 1,
                    y: 0,
                    size: new(width: 70, height: 8)
                ),
                transform: new(position: new(x: 1, y: 1), scale: Scale)
            )
        );

        private readonly Renderer HpBaseRenderer = new(
            new DrawImageCommand(
                texture: TileMap.LoadSprite(
                    path: "Assets/UI/player-bars.png",
                    x: 1,
                    y: 9,
                    size: new(width: 70, height: 7)
                ),
                transform: new(position: new(x: 1, y: 1), scale: Scale)
            )
        );

        private readonly Renderer HpRenderer;
        private readonly Transform HpTransform;

        public HpBar(EventBus bus)
        {
            bus.Subscribe<PlayerDmgEvent>(handler: HandlePlayerDmg);
            bus.Subscribe<PlayerResetEvent>(handler: HandlePlayerReset);

            HpTransform = new(
                position: new(x: 1, y: 1),
                scale: new(width: Scale.Width, height: Scale.Height)
            );
            HpRenderer = new(
                new DrawImageCommand(
                    texture: TileMap.LoadSprite(
                        path: "Assets/UI/player-bars.png",
                        x: 1,
                        y: 17,
                        size: new(width: 70, height: 7)
                    ),
                    transform: HpTransform
                )
            );
        }

        private void HandlePlayerDmg(PlayerDmgEvent data)
        {
            Hp = data.Hp - data.Dmg;
            HpTransform.Scale.Width = Scale.Width * Hp / BaseHp;
        }

        private void HandlePlayerReset(PlayerResetEvent data)
        {
            BaseHp = Hp = data.Hp;
            HpTransform.Scale.Width = Scale.Width;
        }

        public override void Draw()
        {
            HpBaseRenderer.Draw();
            HpRenderer.Draw();
            HpFrameRenderer.Draw();
        }
    }
}
