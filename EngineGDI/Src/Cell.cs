using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace EngineGDI.Src
{
    public class Cell
    {
        public int x;
        public int y;
        public int posX;
        public int posY;

        //public static Cell[,]  grid= new Cell[7, 7];

        //public Image gridTexture = Image.FromFile("Assets/Imgs/gridUndiscovered.png");

        public Cell(int tilesize, int x, int y)
        {
            this.x = x;
            this.y = y;

            posX = x * tilesize;
            posY = y * tilesize;
        }

        public void Pintar()
        {
            Engine.Draw(
                offsetX: 0,
                offsetY: 0,
                texture: gridTexture,
                x: posX,
                y: posY,
                scaleX: 1,
                scaleY: 1,
                angle: 0
            );
        }
    }
}
