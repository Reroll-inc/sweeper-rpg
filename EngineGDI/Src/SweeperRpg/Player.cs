using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.SweeperRpg
{
    public class Player : Node
    {
        private Point position;
        private Point positionToUpdate = new Point();
        private readonly Image tile;

        public Player(int x, int y)
        {
            position = new Point(x: x, y: y);
            tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);

            CollisionManager.RegisterPlayer(position: position);
        }

        public void Reset(int x, int y)
        {
            position.X = x;
            position.Y = y;
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

        public override void Update(float deltaTime)
        {
            positionToUpdate.X = position.X * 32;
            positionToUpdate.Y = position.Y * 32;

            CollisionManager.UpdatePlayer(position: positionToUpdate);
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: tile, x: positionToUpdate.X, y: positionToUpdate.Y);
        }
    }
}
