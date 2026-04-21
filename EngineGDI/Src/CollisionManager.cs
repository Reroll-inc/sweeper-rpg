using System.Collections.Generic;
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

        // public static void RegisterEnemy(Collisioner enemy)
        // {
        //     instance.enemies.Add(enemy);
        // }

        // public static void UnRegisterEnemy(Collisioner enemy)
        // {
        //     if (!instance.enemies.Remove(enemy))
        //         throw new System.Exception("Enemy already removed?");
        // }

        public static void UpdatePlayer(Point position)
        {
            instance.player.UpdatePosition(position: position);

            foreach (Enemy enemy in LevelManager.Instance.ActiveEnemies)
            {
                if (instance.player.CheckCollision(enemy.Collsion))
                {
                    //Notify Enemy
                    LevelManager.Instance.OnCollision(enemy);
                    enemy.Collsion.OnCollisionIn();
                    // Notify Player

                    instance.player.OnCollisionIn();
                    // cambios: En collisioner - en levelmanager - en collisionmanager
                    // Hacer que collisionManager deje de registrar los collisioners enemigos y le pida la lista de enemigos activos al levelmanager
                }
                else
                {
                    enemy.Collsion.OnCollisionOut();
                    instance.player.OnCollisionOut();
                }
            }
        }

        public override void Draw()
        {
            player.Draw();
            foreach (Enemy enemy in LevelManager.Instance.ActiveEnemies)
            {
                enemy.Collsion.Draw();
            }
        }
    }
}
