using System.Drawing;
using System.Numerics;

namespace EngineGDI.Src
{
    public class Enemy : Node
    {
        private Vector2 position;
        private readonly Image tile;

        public Enemy(int x, int y, int row, int column)
        {
            // monsters: 12x13
            position = new Vector2(x: x, y: y);
            tile = TileMap.LoadSprite(
                path: "Assets/32rogues/monsters.png",
                row: row,
                column: column
            );
        }

        public override void Draw()
        {
            Engine.Draw(texture: tile, x: position.X * 32, y: position.Y * 32);
        }
    }
}
