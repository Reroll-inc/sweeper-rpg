namespace EngineGDI
{
    public class Cell
    {
        public int x;
        public int y;
        public int posX;
        public int posY;

        //undiscovered discovered playerIn
        public string states = "Assets/Imgs/gridUndiscovered.png";

        public Cell(int tilesize, int x, int y, string sprite)
        {
            this.x = x;
            this.y = y;

            posX = x * tilesize;
            posY = y * tilesize;

            states = "Assets/Imgs/gridUndiscovered.png";
        }
    }
}
