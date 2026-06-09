using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src;

namespace SweeperRpg.Src
{
    public delegate void PlayerEventWillMove(Point newPosition);
    public delegate void PlayerEventIsDead();

    public class Player : Node
    {
        public event PlayerEventWillMove OnWillMove;
        public event PlayerEventIsDead OnPlayerDeath;

        private Point position;
        public Point Position => position;
        private Point positionToUpdate;
        private Point start;

        public Image Tile { get; }
        private readonly int maxHealth = 8;

        public int Hp { get; private set; }
        public Collisioner Collisioner { get; }
        private Transform transform;

        public Player(int x, int y)
        {
            transform.Position = new Vector2(x, y);
            positionToUpdate.X = position.X * 32;
            positionToUpdate.Y = position.Y * 32;
            Tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);

            Collisioner = new Collisioner(
                position: position,
                size: new Size(width: 32, height: 32)
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

            if (IsDead())
            {
                OnPlayerDeath();
            }
        }

        private bool IsDead()
        {
            return Hp < 0;
        }

        public void Move(Point newPosition)
        {
            position = newPosition;
            positionToUpdate.X = position.X * 32;
            positionToUpdate.Y = position.Y * 32;

            Collisioner.UpdatePosition(position: positionToUpdate);
        }

        public bool Collide(Enemy enemy)
        {
            bool DidCollision = Collisioner.CheckCollision(enemy.Collisioner);

            if (DidCollision)
            {
                TakeDamage(enemy.Damage);

                if (!IsDead())
                {
                    enemy.Defeat();
                }
            }

            return DidCollision;
        }

        public override void Input()
        {
            bool changed = false;
            Point newPosition = new(x: position.X, y: position.Y);

            if (Engine.OnKeyDown(Keys.W))
            {
                newPosition.Y--;
                changed = true;
            }
            if (Engine.OnKeyDown(Keys.A))
            {
                newPosition.X--;
                changed = true;
            }
            if (Engine.OnKeyDown(Keys.S))
            {
                newPosition.Y++;
                changed = true;
            }
            if (Engine.OnKeyDown(Keys.D))
            {
                newPosition.X++;
                changed = true;
            }

            if (changed)
            {
                OnWillMove(newPosition: newPosition);
            }
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: Tile, x: positionToUpdate.X, y: positionToUpdate.Y);
        }
    }
}
