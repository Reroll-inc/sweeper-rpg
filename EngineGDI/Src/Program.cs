using System;
using System.Drawing;
using System.Windows.Forms;

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
        public static int SCREEN_HEIGHT = 544;

        public static Player p1 = new Player(1, 1);

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Initialize("IERVA ENGINE", SCREEN_WIDTH, SCREEN_HEIGHT, false);

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
            p1.Update(deltaTime: deltaTime);
        }

        static void Render()
        {
            p1.Draw();
        }
    }
}
