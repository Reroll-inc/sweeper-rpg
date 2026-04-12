using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace EngineGDI.Src
{
    public class Player : Node
    {
        private Vector2 position;
        private readonly Image tile;

        public Player(int x, int y)
        {
            // 7x7
            position = new Vector2(x: x, y: y);
            tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);
        }

        public override void Input()
        {
            if (Engine.OnKeyDown(Keys.W))
                position.Y--;
            if (Engine.OnKeyDown(Keys.A))
                position.X--;
            if (Engine.OnKeyDown(Keys.S))
                position.Y++;
            if (Engine.OnKeyDown(Keys.D))
                position.X++;
            if (Engine.OnKeyDown(Keys.Space))
            {
                position.X = 1;
                position.Y = 1;
            }
        }

        public override void Draw()
        {
            Engine.Draw(
                texture: tile,
                x: position.X * 32,
                y: position.Y * 32,
                scaleX: 1,
                scaleY: 1,
                angle: 0,
                offsetX: .5f,
                offsetY: .5f
            );
        }
    }
}
