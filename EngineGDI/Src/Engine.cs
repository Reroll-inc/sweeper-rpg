using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using EngineGDI.Src.Drawing;

namespace EngineGDI.Src
{
    public static class Engine
    {
        private static readonly Dictionary<string, SoundPlayer> sounds = [];
        private static readonly List<IDrawCommand> drawQueue = [];
        private static GameForm window;
        public static bool IsWindowOpen { get; private set; }
        public static Form Window => window;

        private static readonly HashSet<Keys> pressedKeys = [];
        private static readonly HashSet<Keys> handledKeys = [];
        private static readonly HashSet<Keys> releasedKeys = [];
        private static readonly HashSet<Keys> handledReleasedKeys = [];

        private static readonly List<string> debugMessages = [];
        private static readonly Font debugFont = new("Consolas", 10);
        private static readonly Brush debugBrush = Brushes.White;

        public static void Initialize(
            string title = "Game",
            int width = 800,
            int height = 600,
            bool fullscreen = false
        )
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            window = new GameForm
            {
                Text = title,
                ClientSize = new Size(width, height),
                StartPosition = FormStartPosition.CenterScreen,
            };
            if (fullscreen)
            {
                window.WindowState = FormWindowState.Maximized;
            }

            window.FormClosed += static (s, e) => IsWindowOpen = false;
            window.KeyDown += static (s, e) =>
            {
                if (!pressedKeys.Contains(e.KeyCode))
                {
                    _ = pressedKeys.Add(e.KeyCode);
                    _ = handledKeys.Remove(e.KeyCode);
                }

                _ = releasedKeys.Remove(e.KeyCode);
                _ = handledReleasedKeys.Remove(e.KeyCode);
            };
            window.KeyUp += static (s, e) =>
            {
                _ = pressedKeys.Remove(e.KeyCode);
                _ = handledKeys.Remove(e.KeyCode);
                _ = releasedKeys.Add(e.KeyCode);
                _ = handledReleasedKeys.Remove(e.KeyCode);
            };
            window.Show();
            _ = window.Focus();
            window.KeyPreview = true;
            IsWindowOpen = true;
        }

        public static void UpdateWindow()
        {
            if (window != null && window.Created)
            {
                Application.DoEvents();
            }
        }

        public static void PlaySound(string path)
        {
            if (!sounds.TryGetValue(path, out SoundPlayer value))
            {
                value = new SoundPlayer(path);
                sounds[path] = value;
            }

            value.Play();
        }

        public static void DrawACommand(IDrawCommand command)
        {
            drawQueue.Add(command);
        }

        public static void DrawRect(Rectangle rect, Pen pen, Brush brush = null)
        {
            drawQueue.Add(
                new DrawRectCommand
                {
                    rect = rect,
                    pen = pen,
                    brush = brush,
                }
            );
        }

        public static void DrawText(string text, Font font, Brush brush, Point position)
        {
            drawQueue.Add(
                new DrawTextCommand
                {
                    text = text,
                    font = font,
                    brush = brush,
                    position = position,
                }
            );
        }

        public static void Clear(Color color)
        {
            window.ClearColor = color;
        }

        public static bool OnKeyDown(Keys key)
        {
            if (pressedKeys.Contains(key) && !handledKeys.Contains(key))
            {
                _ = handledKeys.Add(key);
                return true;
            }
            return false;
        }

        public static bool OnKeyUp(Keys key)
        {
            if (releasedKeys.Contains(key) && !handledReleasedKeys.Contains(key))
            {
                _ = handledReleasedKeys.Add(key);
                return true;
            }
            return false;
        }

        public static bool IsKeyDown(Keys key)
        {
            return pressedKeys.Contains(key);
        }

        // Alias de compatibilidad: mismo comportamiento que OnKeyDown
        public static bool IsKeyPressed(Keys key)
        {
            return OnKeyDown(key);
        }

        public static void DebugLog(string message)
        {
            debugMessages.Add(message);
        }

        public static void ClearDebug()
        {
            debugMessages.Clear();
        }

        private class GameForm : Form
        {
            public Color ClearColor = Color.Black;

            public GameForm()
            {
                DoubleBuffered = true;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.Clear(ClearColor);
                foreach (IDrawCommand cmd in drawQueue)
                {
                    cmd.Draw(e);
                }

                float debugY = 10;
                foreach (string msg in debugMessages)
                {
                    e.Graphics.DrawString(msg, debugFont, debugBrush, 10, debugY);
                    debugY += debugFont.Height + 2;
                }
                drawQueue.Clear();
            }
        }
    }
}
