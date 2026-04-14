using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src
{
    public class Player : Node
    {
        private Point position;
        private readonly Image tile;
        private Point positionToUpdate;

        public Player(int x, int y)
        {
            position = new Point(x: x, y: y);
            tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);

            CollisionManager.RegisterPlayer(position: position);
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
            positionToUpdate = new Point(position.X * 32, position.Y * 32);

            CollisionManager.UpdatePlayer(position: positionToUpdate);
        }

        public override void Draw()
        {
            Engine.Draw(texture: tile, x: positionToUpdate.X, y: positionToUpdate.Y);
        }
    }
}
