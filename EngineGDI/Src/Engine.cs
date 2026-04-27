using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace EngineGDI.Src
{
    public static class Engine
    {
        public abstract class DrawCommand
        {
            public abstract void Draw(PaintEventArgs e);
        }

        private class DrawImageCommand : DrawCommand
        {
            public Image texture;
            public float X,
                Y,
                ScaleX,
                ScaleY;
            public float Angle,
                OffsetX,
                OffsetY;

            public override void Draw(PaintEventArgs e)
            {
                float width = texture.Width * ScaleX;
                float height = texture.Height * ScaleY;

                InterpolationMode prevInterpolation = e.Graphics.InterpolationMode;
                e.Graphics.TranslateTransform(X, Y);
                e.Graphics.RotateTransform(Angle);

                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.DrawImage(texture, -OffsetX * width, -OffsetY * height, width, height);
                e.Graphics.InterpolationMode = prevInterpolation;

                e.Graphics.ResetTransform();
            }
        }

        private class DrawRectCommand : DrawCommand
        {
            public Rectangle rect;
            public Pen pen;
            public Brush brush = null;

            public override void Draw(PaintEventArgs e)
            {
                if (!(brush is null))
                    e.Graphics.FillRectangle(brush, rect);

                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        private class DrawTextCommand : DrawCommand
        {
            public string text;
            public Font font;
            public Brush brush;
            public PointF position;

            public override void Draw(PaintEventArgs e)
            {
                e.Graphics.DrawString(text, font, brush, position);
            }
        }

        private class CollisionCommand
        {
            public Pen pen;
            public Brush brush;
            public Rectangle rect;
        }

        private static readonly Dictionary<string, SoundPlayer> sounds =
            new Dictionary<string, SoundPlayer>();
        private static readonly List<DrawCommand> drawQueue = new List<DrawCommand>();
        private static readonly List<CollisionCommand> collisionQueue =
            new List<CollisionCommand>();
        private static GameForm window;
        public static bool IsWindowOpen { get; private set; } = false;
        public static Form Window => window;

        private static readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private static readonly HashSet<Keys> handledKeys = new HashSet<Keys>();
        private static readonly HashSet<Keys> releasedKeys = new HashSet<Keys>();
        private static readonly HashSet<Keys> handledReleasedKeys = new HashSet<Keys>();

        private static readonly List<string> debugMessages = new List<string>();
        private static readonly Font debugFont = new Font("Consolas", 10);
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
                window.WindowState = FormWindowState.Maximized;
            window.FormClosed += (s, e) => IsWindowOpen = false;
            window.KeyDown += (s, e) =>
            {
                if (!pressedKeys.Contains(e.KeyCode))
                {
                    pressedKeys.Add(e.KeyCode);
                    handledKeys.Remove(e.KeyCode);
                }

                releasedKeys.Remove(e.KeyCode);
                handledReleasedKeys.Remove(e.KeyCode);
            };
            window.KeyUp += (s, e) =>
            {
                pressedKeys.Remove(e.KeyCode);
                handledKeys.Remove(e.KeyCode);
                releasedKeys.Add(e.KeyCode);
                handledReleasedKeys.Remove(e.KeyCode);
            };
            window.Show();
            window.Focus();
            window.KeyPreview = true;
            IsWindowOpen = true;
        }

        public static void UpdateWindow()
        {
            if (window != null && window.Created)
                Application.DoEvents();
        }

        public static void PlaySound(string path)
        {
            if (!sounds.ContainsKey(path))
                sounds[path] = new SoundPlayer(path);
            sounds[path].Play();
        }

        public static void DrawACommand(DrawCommand command)
        {
            drawQueue.Add(command);
        }

        public static void DrawImage(
            Image texture,
            float x,
            float y,
            float scaleX = 1f,
            float scaleY = 1f,
            float angle = 0f,
            float offsetX = 0f,
            float offsetY = 0f
        )
        {
            drawQueue.Add(
                new DrawImageCommand
                {
                    texture = texture,
                    X = x,
                    Y = y,
                    ScaleX = scaleX,
                    ScaleY = scaleY,
                    Angle = angle,
                    OffsetX = offsetX,
                    OffsetY = offsetY,
                }
            );
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

        public static void DrawText(string text, Font font, Brush brush, PointF position)
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

        public static void DrawCollision(Pen pen, Rectangle rect, Brush brush = null)
        {
            // if (debugMessages.i)
            collisionQueue.Add(
                new CollisionCommand
                {
                    pen = pen,
                    brush = brush,
                    rect = rect,
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
                handledKeys.Add(key);
                return true;
            }
            return false;
        }

        public static bool OnKeyUp(Keys key)
        {
            if (releasedKeys.Contains(key) && !handledReleasedKeys.Contains(key))
            {
                handledReleasedKeys.Add(key);
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
                foreach (var cmd in drawQueue)
                    cmd.Draw(e);

                foreach (var collision in collisionQueue)
                {
                    if (!(collision.brush is null))
                        e.Graphics.FillRectangle(collision.brush, collision.rect);
                    e.Graphics.DrawRectangle(collision.pen, collision.rect);
                }
                collisionQueue.Clear();

                float debugY = 10;
                foreach (var msg in debugMessages)
                {
                    e.Graphics.DrawString(msg, debugFont, debugBrush, 10, debugY);
                    debugY += debugFont.Height + 2;
                }
                drawQueue.Clear();
            }
        }
    }
}
