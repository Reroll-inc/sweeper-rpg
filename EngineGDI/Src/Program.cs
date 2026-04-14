using System;
using System.Drawing;
using EngineGDI.Src.UI;

namespace EngineGDI.Src
{
    static class Program
    {
        // Delta time
        public static float deltaTime;
        static DateTime lastFrameTime = DateTime.Now;

        // mostrar debug
        public static bool showDebug = true;
        public static string currentMsg = "";

        public static int SCREEN_WIDTH = 1024;
        public static int SCREEN_HEIGHT = 500;

        private static readonly CollisionManager collisionManager = CollisionManager.Instance();
        private static readonly Node p1 = new Player(x: 1, y: 1);
        private static readonly Node grid = new Grid();
        private static readonly Node[] enemies =
        {
            new Enemy(x: 16, y: 16, 2, 1),
            new Enemy(x: 24, y: 1, 1, 1),
            new Enemy(x: 3, y: 8, 0, 4),
            new Enemy(x: 12, y: 12, 4, 3),
            new Enemy(x: 13, y: 7, 5, 2),
            new Enemy(x: 1, y: 5, 6, 7),
            new Enemy(x: 17, y: 2, 7, 4),
            new Enemy(x: 16, y: 10, 8, 4),
        };

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Initialize("Sweeper RPG", SCREEN_WIDTH, SCREEN_HEIGHT, false);

            while (Engine.IsWindowOpen)
            {
                #region Engine Window Control
                Engine.UpdateWindow();
                #endregion

                CalcDeltatime();

                Input();
                Update();
                Render();

                #region Engine Window Control
                Engine.Clear(Color.Black);
                currentMsg = deltaTime.ToString();
                // mensajes de debug
                if (showDebug)
                {
                    Engine.ClearDebug();
                    Engine.DebugLog(currentMsg);
                }
                Engine.Window.Invalidate();
                #endregion
            }
        }

        static void CalcDeltatime()
        {
            TimeSpan deltaSpan = DateTime.Now - lastFrameTime;
            deltaTime = (float)deltaSpan.TotalSeconds;
            lastFrameTime = DateTime.Now;
        }

        static void Input()
        {
            p1.Input();
        }

        static void Update()
        {
            foreach (Node enemy in enemies)
                enemy.Update(deltaTime: deltaTime);

            p1.Update(deltaTime: deltaTime);

            collisionManager.Update(deltaTime: deltaTime);
        }

        static void Render()
        {
            foreach (Node enemy in enemies)
                enemy.Draw();

            grid.Draw();
            p1.Draw();

            collisionManager.Draw();
        }
    }
}
