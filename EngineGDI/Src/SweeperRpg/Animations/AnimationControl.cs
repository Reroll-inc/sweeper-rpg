using System.Drawing;
using System.Windows.Forms;
using static EngineGDI.Src.Engine;

namespace EngineGDI.Src.SweeperRpg.Animations
{
    public class AnimationControl : DrawCommand
    {
        // 0 = full square, 100 = fully vanished
        private float peelAmount = 0;
        private float time = 0;
        private readonly float sec = (float)0.4;
        private static readonly Font debugFont = new Font("Consolas", 10);

        public override void Draw(PaintEventArgs e)
        {
            int cellSize = 100;
            int x = 50;
            int y = 50;

            // Calculate how much of the corner is cut off
            // At 0% peel: cut = 0 pixels
            // At 100% peel: cut = cellSize pixels
            int cutSize = (int)(cellSize * peelAmount) / 100;

            // Define the 4 points of our polygon
            Point[] points;

            // Cut-off shape (5 points - it's a pentagon)
            points = new Point[]
            {
                new Point(x + cutSize, y), // Top edge moves right
                new Point(x + cellSize, y), // Top-right (unchanged)
                new Point(x + cellSize, y + cellSize), // Bottom-right (unchanged)
                new Point(x, y + cellSize), // Bottom-left (unchanged)
                new Point(x, y + cutSize), // Left edge moves down
            };

            // Draw the polygon
            e.Graphics.FillPolygon(Brushes.LightBlue, points);
            e.Graphics.DrawPolygon(Pens.Black, points);

            // Draw some text to see it better
            e.Graphics.DrawString(
                $"{peelAmount}% {time}s",
                debugFont,
                Brushes.White,
                x + 10,
                y + 40
            );
        }

        public void Update(float deltaTime)
        {
            peelAmount += (100 / sec) * deltaTime;

            if (peelAmount > 100)
                peelAmount = 100;
            else
                time += deltaTime;
        }
    }
}
