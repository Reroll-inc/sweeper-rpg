using System;
using System.Drawing;
using System.IO;
using EngineGDI.Src;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src
{
    internal static class Program
    {
        // Delta time
        public static float deltaTime;

        [PlantUmlIgnoreAssociation]
        private static DateTime lastFrameTime = DateTime.Now;

        public static int SCREEN_WIDTH = 1024;
        public static int SCREEN_HEIGHT = 720;

        private static readonly GameManager gameManager = GameManager.Instance;

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            Engine.Initialize(
                title: "Sweeper RPG",
                width: SCREEN_WIDTH,
                height: SCREEN_HEIGHT,
                fullscreen: false
            );

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
                Engine.Window.Invalidate();
                #endregion
            }
        }

        private static void CalcDeltatime()
        {
            TimeSpan deltaSpan = DateTime.Now - lastFrameTime;
            deltaTime = (float)deltaSpan.TotalSeconds;
            lastFrameTime = DateTime.Now;
        }

        private static void Input()
        {
            gameManager.Input();
        }

        private static void Update()
        {
            gameManager.Update(deltaTime: deltaTime);
        }

        private static void Render()
        {
            gameManager.Draw();
        }
    }
}
