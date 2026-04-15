using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EngineGDI.Src.SweeperRpg
{
    /**
     * El tamaño de la grilla máxima tiene 16x32 cuadrantes de 32px.
     */
    public class Grid : Node
    {
        private readonly List<List<CellData>> level;
        private readonly int MAX_ROW = 16;
        private readonly int MAX_COLUMN = 32;

        private int lvlRow;
        private int lvlColumn;
        private int fillRows;
        private int fillColumns;

        [JsonConverter(typeof(StringEnumConverter))]
        public enum CellType
        {
            [EnumMember(Value = "N")]
            NULL,

            [EnumMember(Value = "E")]
            ENEMY,

            [EnumMember(Value = "C")]
            COIN,

            [EnumMember(Value = "S")]
            START,

            [EnumMember(Value = "D")]
            END,
        }

        public class CellData
        {
            public CellType type = CellType.NULL;
            public string id = null;
            public EnemyKind? kind = null;
            public int currency = 0;
        }

        public Grid()
        {
            level = LoadJson();
            lvlRow = level.Count;
            lvlColumn = level[0].Count;

            if (lvlRow > 16 || lvlColumn > 32)
                throw new System.Exception(
                    $"Level size is [{lvlRow},{lvlColumn}] which is bigger than [16,32]"
                );

            fillRows = (MAX_ROW - lvlRow) / 2;
            fillColumns = (MAX_COLUMN - lvlColumn) / 2;
        }

        private List<List<CellData>> LoadJson()
        {
            string jsonContent = File.ReadAllText("Assets/Levels/1.json");

            return JsonConvert.DeserializeObject<List<List<CellData>>>(jsonContent);
        }

        public override void Update(float deltaTime)
        {
            //aca metemos el posible chequeo de colicion con el player
        }

        public override void Draw()
        {
            // FIXME: Optimizar. Se dibuja todo el fondo para que luego
            // El nivel redibuje las celdas del nivel.
            for (int rowId = 0; rowId < MAX_ROW; rowId++)
            for (int columnId = 0; columnId < MAX_COLUMN; columnId++)
                Engine.DrawRect(
                    rect: new Rectangle(
                        location: new Point(x: columnId * 32, y: rowId * 32),
                        size: new Size(width: 32, height: 32)
                    ),
                    pen: new Pen(color: Color.DarkSlateGray),
                    brush: new SolidBrush(Color.DarkSlateGray)
                );

            for (int rowId = 0; rowId < level.Count; rowId++)
            {
                List<CellData> row = level[rowId];

                for (int columnId = 0; columnId < row.Count; columnId++)
                {
                    CellData cell = row[columnId];

                    switch (cell.type)
                    {
                        case CellType.COIN:
                            //si la celda alguna moneda, los dibujamos :)
                            break;
                        case CellType.ENEMY:

                            //si la celda tiene enemigos, dibujamos la casilla de combate
                            //si distintos assets para distintos enemigos habria que repensarlo
                            //o clavar alguna logica de que el string enemy sea el nombre del asset
                            //mas ppractico
                            break;
                        case CellType.START:
                            Engine.DrawRect(
                                rect: new Rectangle(
                                    location: new Point(
                                        x: (columnId * 32) + (fillColumns * 32),
                                        y: (rowId * 32) + (fillRows * 32)
                                    ),
                                    size: new Size(width: 32, height: 32)
                                ),
                                pen: new Pen(color: Color.Black),
                                brush: new SolidBrush(Color.DarkViolet)
                            );
                            continue;
                        case CellType.END:
                            Engine.DrawRect(
                                rect: new Rectangle(
                                    location: new Point(
                                        x: (columnId * 32) + (fillColumns * 32),
                                        y: (rowId * 32) + (fillRows * 32)
                                    ),
                                    size: new Size(width: 32, height: 32)
                                ),
                                pen: new Pen(color: Color.Black),
                                brush: new SolidBrush(Color.DarkGoldenrod)
                            );
                            continue;
                        case CellType.NULL:
                            break;
                    }

                    Engine.DrawRect(
                        rect: new Rectangle(
                            location: new Point(
                                x: (columnId * 32) + (fillColumns * 32),
                                y: (rowId * 32) + (fillRows * 32)
                            ),
                            size: new Size(width: 32, height: 32)
                        ),
                        pen: new Pen(color: Color.Black),
                        brush: new SolidBrush(Color.SeaGreen)
                    );
                }
            }
        }
    }
}
