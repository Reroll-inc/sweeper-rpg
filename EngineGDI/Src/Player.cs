using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI.Src
{
    public class Player
    {
        private int posX;
        private int posY;
        private Image tile;

        public Player(int posX, int posY)
        {
            ReadSprite(2, 2);

            this.posX = posX;
            this.posY = posY;
        }

        private void ReadSprite(int row, int column)
        {
            // 7x7 32px
            if (row >= 7 || column >= 7)
                throw new System.Exception("Player sprites row/column valid values are [0, 6]");

            using (Bitmap sourceImage = new Bitmap("Assets/32rogues/rogues.png"))
            {
                int tileSize = 32;

                // Calculate the rectangle for the specific tile
                Rectangle cropRect = new Rectangle(
                    column * tileSize,
                    row * tileSize,
                    tileSize,
                    tileSize
                );

                // Clone the specific portion of the image
                tile = sourceImage.Clone(cropRect, sourceImage.PixelFormat);
            }
        }

        public void Input()
        {
            if (Engine.OnKeyDown(Keys.W))
                posY--;
            if (Engine.OnKeyDown(Keys.A))
                posX--;
            if (Engine.OnKeyDown(Keys.S))
                posY++;
            if (Engine.OnKeyDown(Keys.D))
                posX++;
            if (Engine.OnKeyDown(Keys.Space))
            {
                posX = 1;
                posY = 1;
            }
        }

        public void Update(float deltaTime)
        {
            // posX = (posX + 1) % 1024;
        }

        public void Draw()
        {
            Engine.Draw(
                texture: tile,
                x: posX * 32,
                y: posY * 32,
                scaleX: 1,
                scaleY: 1,
                angle: 0,
                offsetX: .5f,
                offsetY: .5f
            );
        }
    }
}
