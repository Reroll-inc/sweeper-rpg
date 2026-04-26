using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.SweeperRpg
{
    public class Player : Node
    {
        private Point position;
        private Point positionToUpdate = new Point();
        private readonly Image tile;
        private int maxHealth = 8;
        private int health;

        public Player(int x, int y)
        {
            position = new Point(x: x, y: y);
            positionToUpdate.X = position.X * 32;
            positionToUpdate.Y = position.Y * 32;
            tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);

            CollisionManager.RegisterPlayer(position: position);
        }

        public void Reset(int x, int y)
        {
            position.X = x;
            position.Y = y;

            positionToUpdate.X = position.X * 32;
            positionToUpdate.Y = position.Y * 32;

            health = maxHealth;
        }

        public override void Input()
        {
            bool changed = false;
            Point prevPosition = new Point(x: position.X, y: position.Y);

            if (Engine.OnKeyDown(Keys.W))
            {
                position.Y--;

                changed = true;
            }
            if (Engine.OnKeyDown(Keys.A))
            {
                position.X--;
                changed = true;
            }
            if (Engine.OnKeyDown(Keys.S))
            {
                position.Y++;
                changed = true;
            }
            if (Engine.OnKeyDown(Keys.D))
            {
                position.X++;
                changed = true;
            }

            if (changed)
            {
                if (LevelManager.Instance.IsWithinLimits(position: position))
                {
                    positionToUpdate.X = position.X * 32;
                    positionToUpdate.Y = position.Y * 32;

                    CollisionManager.UpdatePlayer(position: positionToUpdate);
                }
                else
                    position = prevPosition;
            }
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: tile, x: positionToUpdate.X, y: positionToUpdate.Y);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        public bool IsDead()
        {
            return health <= 0;
        }
    }
}
