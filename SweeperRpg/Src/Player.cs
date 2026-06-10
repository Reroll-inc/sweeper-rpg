using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src
{
    public delegate void PlayerEventWillMove(Point newPosition);
    public delegate void PlayerEventIsDead();

    public class Player : IInteractiveNode
    {
        public event PlayerEventWillMove OnWillMove;
        public event PlayerEventIsDead OnDeath;

        private Point position;
        public Point Position => position;
        private Point positionToUpdate;
        private Point start;

        public Image Tile { get; }
        private readonly int maxHealth = 8;

        public int Hp { get; private set; }
        public Collisioner Collisioner { get; }

        private readonly Transform transform;

        public Player(int x, int y)
        {
            transform = new(new Vector2(x, y), 0, new Vector2(1, 1));
            Collisioner = new Collisioner(
                position: new Point(0, 0),
                size: new Size(width: 32, height: 32)
            );
            Tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);

            SetStart(x: x, y: y);
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

            Collisioner.UpdatePosition(positionToUpdate);
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;

            if (IsDead())
            {
                OnDeath();
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

        public bool TryCollide(Enemy enemy)
        {
            bool DidCollision = Collisioner.CheckCollision(enemy.Collisioner);

            if (DidCollision)
            {
                TakeDamage(enemy.Damage);
                enemy.Defeat();
            }

            return DidCollision;
        }

        public void Input()
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

        public void Update(float deltaTime) { }

        public void Draw()
        {
            Engine.DrawImage(texture: Tile, x: positionToUpdate.X, y: positionToUpdate.Y);
        }
    }
}
