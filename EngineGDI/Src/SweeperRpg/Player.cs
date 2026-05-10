using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.SweeperRpg
{
    public class Player : Node
    {
        private Point position;
        public Point Position => position;
        private Point positionToUpdate;
        private Point start;

        public Image Tile { get; }
        private readonly int maxHealth = 8;

        public int Hp { get; private set; }
        public Collisioner Collisioner { get; }

        public Player(int x, int y)
        {
            position = new Point(x: x, y: y);
            positionToUpdate.X = position.X * 32;
            positionToUpdate.Y = position.Y * 32;
            Tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);

            Collisioner = new Collisioner(
                position: position,
                size: new Size(width: 32, height: 32),
                brushColor: Color.DarkBlue
            );
        }

        public void SetStart(int x, int y)
        {
            start.X = x;
            start.Y = y;

            Reset();
        }

        public void Reset()
        {
            position.X = start.X;
            position.Y = start.Y;
            positionToUpdate.X = position.X * 32;
            positionToUpdate.Y = position.Y * 32;

            Hp = maxHealth;

            Collisioner.Reset(positionToUpdate);
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
        }

        public bool IsDead()
        {
            return Hp < 0;
        }

        public override void Input()
        {
            bool changed = false;
            Point prevPosition = new(x: position.X, y: position.Y);

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

                    Collisioner.UpdatePosition(position: positionToUpdate);

                    CollisionManager.ValidateCollitions();

                    LevelManager.Instance.CheckVictoryCondition();
                }
                else
                {
                    position = prevPosition;
                }
            }
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: Tile, x: positionToUpdate.X, y: positionToUpdate.Y);
        }
    }
}
