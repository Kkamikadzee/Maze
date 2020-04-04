using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Maze
    {
        public MazeCell[] Cells;
        public MazeCell FinishCell;

        public byte[,] AdjacencMatrix;
        public int AmountCells => Cells.Length;

        private int indexStartCell = -1;
        public int IndexStartCell
        {
            get
            {
                if (indexStartCell != -1)
                {
                    return indexStartCell;
                }
                else
                {
                    foreach ((MazeCell cell, int index) in Cells.Select((value, i) => (value, i)))
                    {
                        if (cell.position == PositionStart)
                        {
                            indexStartCell = index;
                            return index;
                        }
                    }
                }
                return -1;
            }
        }
        public Vector2 PositionStart;
        public Vector2 PositionFinish;
        public MazeCell StartCell
        {
            get
            {
                foreach((MazeCell cell, int index) in Cells.Select((value, i) => (value, i)))
                {
                    if(cell.position == PositionStart)
                    {
                        indexStartCell = index;
                        return cell;
                    }
                }
                return null;
            }
        }

        public Maze() { }

        public void InitializeOrthogonalMaze(int width, int height, Vector2Int positionStart)
        {
            this.PositionStart = positionStart;
            Cells =  new MazeCell[width * height];
            AdjacencMatrix = new byte[AmountCells, AmountCells];

            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    #region Заполнение матрицы смежности
                    if (x - 1 >= 0)
                    {
                        AdjacencMatrix[(x) + width * (y), (x - 1) + width * (y)] = 1;
                        AdjacencMatrix[(x - 1) + width * (y), (x) + width * (y)] = 1;
                    }
                    if (x + 1 < width)
                    {
                        AdjacencMatrix[(x) + width * (y), (x + 1) + width * (y)] = 1;
                        AdjacencMatrix[(x + 1) + width * (y), (x) + width * (y)] = 1;
                    }
                    if (y - 1 >= 0)
                    {
                        AdjacencMatrix[(x) + width * (y), (x) + width * (y - 1)] = 1;
                        AdjacencMatrix[(x) + width * (y - 1), (x) + width * (y)] = 1;
                    }
                    if (y + 1 < height)
                    {
                        AdjacencMatrix[(x) + width * (y), (x) + width * (y + 1)] = 1;
                        AdjacencMatrix[(x) + width * (y + 1), (x) + width * (y)] = 1;
                    }
                    #endregion
                    Cells[x + width * y] = new MazeCell(new Vector2(x, y), 4);
                }
            }
        }
    }
}
