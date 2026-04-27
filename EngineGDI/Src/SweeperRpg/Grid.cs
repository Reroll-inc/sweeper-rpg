using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using EngineGDI.Src.SweeperRpg.Animations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static EngineGDI.Src.SweeperRpg.Enemy;

namespace EngineGDI.Src.SweeperRpg
{
    /**
     * El tamaño de la grilla máxima tiene 16x32 cuadrantes de 32px.
     */
    public class Grid : Node
    {
        private readonly int MAX_ROW = 16;
        private readonly int MAX_COLUMN = 32;

        private LevelData level;
        private int lvlRow;
        private int lvlColumn;
        private int fillRows;
        private int fillColumns;

        public Grid() { }

        public void SetLevel(LevelData level)
        {
            this.level = level;

            lvlRow = level.grid.Count;
            lvlColumn = level.grid[0].Count;

            if (lvlRow > MAX_ROW || lvlColumn > MAX_COLUMN)
                throw new Exception(
                    $"Level size is [{lvlRow},{lvlColumn}] which is bigger than [16,32]"
                );

            fillRows = (MAX_ROW - lvlRow) / 2;
            fillColumns = (MAX_COLUMN - lvlColumn) / 2;

            for (int rowId = 0; rowId < level.grid.Count; rowId++)
            {
                List<Cell> row = level.grid[rowId];

                for (int columnId = 0; columnId < row.Count; columnId++)
                {
                    Cell cell = row[columnId];

                    cell.SetData(
                        level: level,
                        columnId: columnId,
                        rowId: rowId,
                        fillColumns: fillColumns,
                        fillRows: fillRows
                    );
                }
            }
        }

        public void Reset()
        {
            foreach (List<Cell> row in level.grid)
            foreach (Cell cell in row)
                cell.Reset();
        }

        public override void Update(float deltaTime)
        {
            //aca metemos el posible chequeo de colicion con el player
            foreach (List<Cell> row in level.grid)
            foreach (Cell cell in row)
                cell.Update(deltaTime: deltaTime);
        }

        public override void Draw()
        {
            // Draw filling cells
            for (int rowId = 0; rowId < MAX_ROW; rowId++)
            for (int columnId = 0; columnId < MAX_COLUMN; columnId++)
                Engine.DrawRect(
                    rect: new Rectangle(
                        location: new Point(x: columnId * Cell.SIZE, y: rowId * Cell.SIZE),
                        size: new Size(width: Cell.SIZE, height: Cell.SIZE)
                    ),
                    pen: new Pen(color: Color.DarkSlateGray),
                    brush: new SolidBrush(Color.DarkSlateGray)
                );

            // Draw game cells
            foreach (List<Cell> row in level.grid)
            foreach (Cell cell in row)
                cell.Draw();
        }

        public void DrawAfter()
        {
            foreach (List<Cell> row in level.grid)
            foreach (Cell cell in row)
                cell.DrawAfter();
        }
    }

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

    public class Cell
    {
        private enum State
        {
            INIT = 'I',
            OPENING = 'O',
            OPEN = 'N',
        }

        public CellType type = CellType.NULL;
        public string id = null;
        public EnemyKind? kind = null;
        public int currency = 0;
        public static readonly int SIZE = 32;

        private Rectangle rect;
        public Rectangle Rect
        {
            get { return rect; }
        }

        private State state = State.OPENING;
        private LevelData level;
        private readonly PeelingCellAnimation animation = new PeelingCellAnimation();

        public void SetData(LevelData level, int columnId, int rowId, int fillColumns, int fillRows)
        {
            this.level = level;

            Point point = new Point(
                x: (columnId * SIZE) + (fillColumns * SIZE),
                y: (rowId * SIZE) + (fillRows * SIZE)
            );
            Size size = new Size(SIZE, SIZE);

            rect = new Rectangle(location: point, size: size);

            animation.SetData(point: point, size: size);

            animation.OnFinish += FinishOpening;
        }

        public void StartOpening()
        {
            state = State.OPENING;
        }

        public void FinishOpening(object sender, EventArgs e)
        {
            state = State.OPEN;
        }

        public void Reset()
        {
            // TODO> This should reset to INIT
            state = State.OPENING;

            animation.Reset();
        }

        public void Update(float deltaTime)
        {
            switch (state)
            {
                case State.OPENING:
                    animation.Update(deltaTime: deltaTime);
                    break;
            }
        }

        public void Draw()
        {
            switch (type)
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
                        rect: Rect,
                        pen: new Pen(color: Color.Black),
                        brush: new SolidBrush(level.props.start)
                    );
                    return;
                case CellType.END:
                    Engine.DrawRect(
                        rect: Rect,
                        pen: new Pen(color: Color.Black),
                        brush: new SolidBrush(Color.DarkGoldenrod)
                    );
                    return;
                case CellType.NULL:
                    break;
            }

            Engine.DrawRect(
                rect: Rect,
                pen: new Pen(color: Color.Black),
                brush: new SolidBrush(Color.SeaGreen)
            );
        }

        public void DrawAfter()
        {
            switch (state)
            {
                case State.OPENING:
                    Engine.DrawACommand(animation);
                    break;
            }
        }
    }
}
