using System;
using System.Drawing;
using EngineGDI.Src.SweeperRpg;

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
        public static int SCREEN_HEIGHT = 720;

        private static readonly CollisionManager collisionManager = CollisionManager.Instance;
        private static readonly LevelManager levelManager = LevelManager.Instance;
        private static readonly Node grid = new Grid();

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Initialize(
                title: "Sweeper RPG",
                width: SCREEN_WIDTH,
                height: SCREEN_HEIGHT,
                fullscreen: false
            );

            levelManager.LoadLevel(1);
            levelManager.CreateLevel();

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
            levelManager.Input();
        }

        static void Update()
        {
            levelManager.Update(deltaTime: deltaTime);

            collisionManager.Update(deltaTime: deltaTime);
        }

        static void Render()
        {
            grid.Draw();

            levelManager.Draw();

            collisionManager.Draw();
        }
    }
}
