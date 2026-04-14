using System.Drawing;

namespace EngineGDI.Src.SweeperRpg
{
    public class Enemy : Node
    {
        private Point position;
        private Point actualPosition;
        private readonly Image tile;
        private readonly Collisioner collisioner;

        public Enemy(int x, int y, int row, int column)
        {
            tile = TileMap.LoadSprite(
                path: "Assets/32rogues/monsters.png",
                row: row,
                column: column
            );
            position = new Point(x: x, y: y);
            actualPosition = new Point(x: position.X * 32, y: position.Y * 32);

            collisioner = new Collisioner(
                position: actualPosition,
                size: new Size(width: 32, height: 32),
                brushColor: Color.BlanchedAlmond
            );

            CollisionManager.RegisterEnemy(enemy: collisioner);
        }

        public override void Draw()
        {
            Engine.Draw(texture: tile, x: actualPosition.X, y: actualPosition.Y);
        }
    }
}
