using System.Drawing;
using System.Windows.Forms;
using EngineGDI.Src;
using EngineGDI.Src.Drawing;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src
{
    public class PlayerMoveEvent(Point position) : Event
    {
        [PlantUmlIgnoreAssociation]
        public readonly Point Position = position;
    }

    public class PlayerDmgEvent(int hp, int dmg) : Event
    {
        public readonly int Hp = hp;
        public readonly int Dmg = dmg;
    }

    public class PlayerResetEvent(int hp) : Event
    {
        public readonly int Hp = hp;
    }

    public class PlayerDiedEvent : Event { }

    public class Player : InteractiveNode
    {
        [PlantUmlIgnoreAssociation]
        private Point start;

        [PlantUmlIgnoreAssociation]
        public Image Tile { get; }
        private readonly int maxHealth = 8;

        public int Hp { get; private set; }
        public Collisioner Collisioner { get; }

        public Transform Transform { get; }
        private readonly Renderer renderer;

        private readonly EventBus bus;

        public Player(EventBus bus, int x, int y)
        {
            this.bus = bus;
            Tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);
            Transform = new(position: new Point(x, y));
            renderer = new(command: new DrawImageCommand(texture: Tile, transform: Transform));
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

            bus.Publish<PlayerResetEvent>(new(hp: Hp));
        }

        public void TakeDamage(int damage)
        {
            bus.Publish<PlayerDmgEvent>(new(hp: Hp, dmg: damage));

            Hp -= damage;

            if (IsDead())
            {
                bus.Publish<PlayerDiedEvent>(new());
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
                bus.Publish<PlayerMoveEvent>(new(position: newPosition));
            }
        }

        public override void Draw()
        {
            renderer.Draw();
        }
    }
}
