using EngineGDI.Src.SweeperRpg;

namespace EngineGDI.Src
{
    public class CollisionEvent
    {
        public readonly string id;
    }

    public class CollisionManager : Node
    {
        private static readonly CollisionManager instance = new CollisionManager();

        private CollisionManager() { }

        public static CollisionManager Instance
        {
            get { return instance; }
        }

        public void ValidateCollitions()
        {
            Player player = LevelManager.Instance.Player;

            foreach (Enemy enemy in LevelManager.Instance.ActiveEnemies)
            {
                if (player.Collisioner.CheckCollision(enemy.Collisioner))
                {
                    // Notifico al enemigo
                    enemy.Collisioner.OnCollisionIn();
                    // Notifico al player
                    player.Collisioner.OnCollisionIn();
                    // Notifico al LevelManager
                    LevelManager.Instance.OnCollision(enemy);
                }
                else
                {
                    enemy.Collisioner.OnCollisionOut();
                    player.Collisioner.OnCollisionOut();
                }
            }
        }

        public override void Draw()
        {
            LevelManager.Instance.Player.Collisioner.Draw();

            foreach (Enemy enemy in LevelManager.Instance.ActiveEnemies)
                enemy.Collisioner.Draw();
        }
    }
}
