using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;

namespace EngineGDI.Src
{
    public class Grid : Node
    {
        public Image undiscoveredCell = Image.FromFile("Assets/Imgs/gridUndiscovered.png");
        public Image discoveredCell = Image.FromFile("Assets/Imgs/gridDiscovered.png");
        public Image playerOnCell = Image.FromFile("Assets/Imgs/gridPlayer.png");
        private List<List<CellData>> level;

        private class CellData
        {
            public string type = "NULL";
            public string id = "NULL";
            public string enemy = "NULL";
            public int currency = 0;
        }

        public Grid()
        {
            //aca metemos la logica de generacion del mapa
            LoadJson();
        }

        private void LoadJson()
        {
            string jsonContent = File.ReadAllText("Assets/Levels/1.json");

            level = JsonConvert.DeserializeObject<List<List<CellData>>>(jsonContent);
        }

        public override void Update(float deltaTime)
        {
            //aca metemos el posible chequeo de colicion con el player
            base.Update(deltaTime);
        }

        public override void Draw()
        {
            for (int y = 0; y < level.Count; y++)
            {
                for (int x = 0; x < level[y].Count; x++)
                {
                    if (level[y][x].currency >= 0)
                    {
                        //si la celda alguna moneda, los dibujamos :)
                    }
                    if (level[y][x].enemy != "NULL")
                    {
                        //si la celda tiene enemigos, dibujamos la casilla de combate
                        //si distintos assets para distintos enemigos habria que repensarlo
                        //o clavar alguna logica de que el string enemy sea el nombre del asset
                        //mas ppractico
                    }
                    if (level[y][x].type == "START")
                    {
                        //dibujar la entrada
                    }
                    else if (level[y][x].type == "END")
                    {
                        //dibujar salida
                    }

                    Engine.Draw(texture: undiscoveredCell, x: x * 32, y: y * 32);
                }
            }
        }
    }
}
