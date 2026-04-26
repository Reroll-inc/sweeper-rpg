using System.Drawing;
using EngineGDI.Src.SweeperRpg;

namespace EngineGDI.Src
{
    public class CollisionEvent
    {
        public readonly string id;
    }

    public class CollisionManager : Node
    {
        private Collisioner player;
        private static readonly CollisionManager instance = new CollisionManager();

        private CollisionManager() { }

        public static CollisionManager Instance
        {
            get { return instance; }
        }

        public static void RegisterPlayer(Point position)
        {
            instance.player = new Collisioner(
                position: position,
                size: new Size(width: 32, height: 32),
                brushColor: Color.DarkBlue
            );
        }

        public static void UpdatePlayer(Point position)
        {
            instance.player.UpdatePosition(position: position);

            foreach (Enemy enemy in LevelManager.Instance.ActiveEnemies)
            {
                if (instance.player.CheckCollision(enemy.Collisioner))
                {
                    // Notifico al enemigo
                    enemy.Collisioner.OnCollisionIn();
                    // Notifico al player
                    instance.player.OnCollisionIn();
                    // Notifico al LevelManager
                    LevelManager.Instance.OnCollision(enemy);
                }
                else
                {
                    enemy.Collisioner.OnCollisionOut();
                    instance.player.OnCollisionOut();
                }
            }
        }

        public override void Draw()
        {
            player.Draw();

            foreach (Enemy enemy in LevelManager.Instance.ActiveEnemies)
                enemy.Collisioner.Draw();
        }
    }
}
