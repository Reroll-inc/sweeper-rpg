using System;
using System.Drawing;

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

        private static readonly Node p1 = new Player(x: 1, y: 1);
        private static readonly Enemy[] enemies =
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
        public static Cell[,] grid = new Cell[7, 7];
        private static Image gridTexture = Image.FromFile("Assets/Imgs/gridUndiscovered.png");

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Initialize("Sweeper RPG", SCREEN_WIDTH, SCREEN_HEIGHT, false);

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    grid[x, y] = new Cell(48, x, y, "Assets/Imgs/gridUndiscovered.png");
                }
            }
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
            foreach (Enemy enemy in enemies)
                enemy.Update(deltaTime: deltaTime);

            p1.Update(deltaTime: deltaTime);
        }

        static void Render()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Cell g = grid[x, y];

                    Engine.Draw(
                        offsetX: 0,
                        offsetY: 0,
                        texture: gridTexture,
                        x: g.posX,
                        y: g.posY,
                        scaleX: 1,
                        scaleY: 1,
                        angle: 0
                    );
                }
            }
            foreach (Enemy enemy in enemies)
                enemy.Draw();

            p1.Draw();
        }
    }
}
