using System.Collections.Generic;
using System.Drawing;

namespace EngineGDI.Src
{
    public class CollisionEvent
    {
        public readonly string id;
    }

    public class CollisionManager : Node
    {
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
        }

        public static void RegisterEnemy(Collisioner enemy)
        {
            self.enemies.Add(enemy);
        }

        public static void UnRegisterEnemy(Collisioner enemy)
        {
            if (!self.enemies.Remove(enemy))
                throw new System.Exception("Enemy already removed?");
        }

        public static void UpdatePlayer(Point position)
        {
            self.player.UpdatePosition(position: position);

            foreach (Collisioner enemy in self.enemies)
            {
                if (self.player.CheckCollision(enemy))
                    enemy.OnCollisionIn();
                else
                    enemy.OnCollisionOut();
            }
        }

        public override void Draw()
        {
            player.Draw();
            foreach (Collisioner enemy in enemies)
            {
                enemy.Draw();
            }
        }
    }
}
