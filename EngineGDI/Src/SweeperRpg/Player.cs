using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src.SweeperRpg
{
    public class Player : Node
    {
        private Point position;
        public Point Position
        {
            get { return position; }
        }
        private Point positionToUpdate = new Point();
        private Point start = new Point();
        private readonly Image tile;
        public Image Tile
        {
            get { return tile; }
        }
        private readonly int maxHealth = 8;
        private int health;
        public int Hp
        {
            get { return health; }
        }

        private readonly Collisioner collisioner;
        public Collisioner Collisioner
        {
            get { return collisioner; }
        }

        public Player(int x, int y)
        {
            position = new Point(x: x, y: y);
            positionToUpdate.X = position.X * 32;
            positionToUpdate.Y = position.Y * 32;
            tile = TileMap.LoadSprite(path: "Assets/32rogues/rogues.png", row: 2, column: 2);

            collisioner = new Collisioner(
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

            health = maxHealth;

            collisioner.Reset(positionToUpdate);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        public bool IsDead()
        {
            return health < 0;
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

                    collisioner.UpdatePosition(position: positionToUpdate);
                }
                else
                    position = prevPosition;
            }
        }

        public override void Draw()
        {
            Engine.DrawImage(texture: tile, x: positionToUpdate.X, y: positionToUpdate.Y);
        }
    }
}
