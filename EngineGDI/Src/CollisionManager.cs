using System.Collections.Generic;
using System.Drawing;

namespace EngineGDI.Src
{
    public class CollisionEvent
    {
        public readonly string id;
    }

    public class CollisionManager
    {
        public event System.EventHandler<CollisionEvent> CollisionIn;
        public event System.EventHandler<CollisionEvent> CollisionOut;
        private readonly List<Collisioner> enemies = new List<Collisioner>();
        private Collisioner player;
        private static CollisionManager self;

        private CollisionManager() { }

        public static CollisionManager Instance()
        {
            if (self == null)
            {
                self = new CollisionManager();
            }

            return self;
        }

        public static void RegisterPlayer(Point position)
        {
            self.player = new Collisioner(
                position: position,
                size: new Size(width: 32, height: 32),
                brushColor: Color.DarkBlue
            );

            self.CollisionIn += self.player.OnCollisionIn;
            self.CollisionOut += self.player.OnCollisionOut;
        }

        public static void RegisterEnemy(Collisioner enemy)
        {
            self.enemies.Add(enemy);

            self.CollisionIn += enemy.OnCollisionIn;
            self.CollisionOut += enemy.OnCollisionOut;
        }

        public static void UnRegisterEnemy(Collisioner enemy)
        {
            if (!self.enemies.Remove(enemy))
                throw new System.Exception("Enemy already removed?");
        }

        public static void UpdatePlayer(Point position)
        {
            self.player.UpdatePosition(position: position);
        }

        public void Update(float deltaTime)
        {
            foreach (Collisioner enemy in enemies)
            {
                if (player.ChecCollision(enemy))
                {
                    OnCollisionIn();

                    return;
                }
            }

            OnCollisionOut();
        }

        public void Draw()
        {
            player.Draw();
            foreach (Collisioner enemy in enemies)
            {
                enemy.Draw();
            }
        }

        private void OnCollisionIn()
        {
            CollisionIn?.Invoke(this, new CollisionEvent());
        }

        private void OnCollisionOut()
        {
            CollisionOut?.Invoke(this, new CollisionEvent());
        }
    }
}
