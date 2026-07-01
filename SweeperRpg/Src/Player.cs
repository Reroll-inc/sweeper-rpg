using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Nodes;

namespace SweeperRpg.Src
{
    public delegate void PlayerEventWillMove(Point newPosition);
    public delegate void PlayerEventIsDead();

    public class Player : InteractiveNode
    {
        public event PlayerEventWillMove OnWillMove;
        public event PlayerEventIsDead OnDeath;

        private Point start;

        public Image Tile { get; }
        private readonly int maxHealth = 8;

        public int Hp { get; private set; }
        public Collisioner Collisioner { get; }

        public Transform Transform { get; }
        private readonly Renderer Renderer;

        public Player(int x, int y)
        {
            Tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);
            Transform = new(position: new Point(x, y));
            Renderer = new(command: new DrawImageCommand(texture: Tile, transform: Transform));
            Collisioner = new Collisioner(transform: Transform);

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
            Transform.Position.X = start.X;
            Transform.Position.Y = start.Y;

            Hp = maxHealth;

            Collisioner.UpdatePosition(transform: Transform);
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
            Transform.Position.X = newPosition.X;
            Transform.Position.Y = newPosition.Y;

            Collisioner.UpdatePosition(transform: Transform);
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

        public override void Input()
        {
            bool changed = false;
            Point newPosition = new(x: Transform.Position.X, y: Transform.Position.Y);

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
            Renderer.Draw();
        }
    }
}
