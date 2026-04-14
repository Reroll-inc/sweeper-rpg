using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EngineGDI.Src
{
    public class Grid
    {
        private List<List<CellData>> level;

        public Grid()
        {
            LoadJson();
        }

        private class CellData
        {
            public string id;
            public string type = "NULL";
        }

        private void LoadJson()
        {
            string jsonContent = File.ReadAllText("Assets/Levels/1.json");

            level = JsonConvert.DeserializeObject<List<List<CellData>>>(jsonContent);
        }
    }
}

// /*for (int x = 0; x < 7; x++)
//             {
//                 for (int y = 0; y < 7; y++)
//                 {
//                     grid[x, y] = new Cell(48, x, y, "Assets/Imgs/gridUndiscovered.png");
//                 }
//             }*/
