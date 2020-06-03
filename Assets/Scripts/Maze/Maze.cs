using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Maze
{
    public abstract class Maze
    {
        protected TessellationType _tessellation;
        protected MazeCell[] _cells;
        protected byte[,] _adjacencMatrix;
        protected int _indexStartCell;
        protected int _indexFinishCell;

        public TessellationType Tessellation => _tessellation;
        public int AmountCells => _cells.Length;

        public int IndexStartCell
        {
            get => _indexStartCell;
            set => SetIndexStartCell(value);
        }

        public MazeCell StartCell
        {
            get => _cells[_indexStartCell];
            set => SetIndexStartCell(value);
        }

        public int IndexFinishCell
        {
            get => _indexFinishCell;
            set => SetIndexFinishCell(value);
        }

        public MazeCell FinishCell
        {
            get => _cells[_indexFinishCell];
            set => SetIndexFinishCell(value);
        }

        public abstract void InitializeAdjacencMatrix();

        private void SetIndexStartCell(int value)
        {
            if (value >= AmountCells)
            {
                throw new Exception("Index start cell can't be larger than Amount Cells");
            }
            else if (value < 0)
            {
                throw new Exception("Index start cell can't be less than zero");
            }
            else
            {
                _indexStartCell = value;
            }
        }

        private void SetIndexStartCell(MazeCell value)
        {
            for (int i = 0; i < AmountCells; i++)
            {
                if (_cells[i] == value)
                {
                    _indexStartCell = i;
                    return;
                }
            }

            throw new Exception("Adjusted cell not found in cell array");
        }

        private void SetIndexFinishCell(int value)
        {
            if (value >= AmountCells)
            {
                throw new Exception("Index finish cell can't be larger than Amount Cells");
            }
            else if (value < 0)
            {
                throw new Exception("Index finish cell can't be less than zero");
            }
            else
            {
                _indexFinishCell = value;
            }
        }

        private void SetIndexFinishCell(MazeCell value)
        {
            for (int i = 0; i < AmountCells; i++)
            {
                if (_cells[i] == value)
                {
                    _indexFinishCell = i;
                    return;
                }
            }

            throw new Exception("Adjusted cell not found in cell array");
        }

        private void CheckIndexCell(int indexCell)
        {
            if (indexCell >= AmountCells)
            {
                throw new Exception("Index cell can't be larger than Amount Cells");
            }
            else if (indexCell < 0)
            {
                throw new Exception("Index cell can't be less than zero");
            }
        }

        public void UnlinkCells(int indexCell1, int indexCell2)
        {
            _adjacencMatrix[indexCell1, indexCell2] = 0;
            _adjacencMatrix[indexCell2, indexCell1] = 0;
        }

        public void UnlinkCells(MazeCell cell1, MazeCell cell2)
        {
            int indexCell1 = -1, indexCell2 = -1;
            for (int i = 0; i < AmountCells; i++)
            {
                if (indexCell1 == -1)
                {
                    if (_cells[i] == cell1)
                    {
                        indexCell1 = i;
                    }
                }

                if (indexCell2 == -1)
                {
                    if (_cells[i] == cell2)
                    {
                        indexCell2 = i;
                    }
                }

                if ((indexCell1 != -1) && (indexCell2 != -1))
                {
                    break;
                }
            }

            UnlinkCells(indexCell1, indexCell2);
        }

        public void SetFinishInFurthestCell()
        {
            var maxDistanceFromStart = _cells.Max(x => x.DistanceFromStart);
            var indexMaxDistanceFromStart = _cells.Select((item, index) => new {Item = item, Index = index})
                .Where(v => v.Item.DistanceFromStart == maxDistanceFromStart)
                .Select(v => v.Index);

            foreach (var value in indexMaxDistanceFromStart)
            {
                _indexFinishCell = value;
                return;
            }
        }

        public List<(int, byte)> GetIndicesNeighboursCell(int indexCell)
        {
            CheckIndexCell(indexCell);
            
            var list = new List<(int, byte)>(_cells[indexCell].AmountNeighbours);
            for (int i = 0; i < AmountCells; i++)
            {
                if (_adjacencMatrix[indexCell, i] != 0)
                {
                    list.Add((i, _adjacencMatrix[indexCell, i]));
                }
            }

            return list;
        }

        public void SetDistanceFromStartForCell(int indexCell, uint distanceFromStart)
        {
            CheckIndexCell(indexCell);

            _cells[indexCell].DistanceFromStart = distanceFromStart;
        }

        public uint GetDistanceFromStartForCell(int indexCell)
        {
            CheckIndexCell(indexCell);

            return _cells[indexCell].DistanceFromStart;
        }

        public bool ThereIsWall(int indexCell1, int indexCell2)
        {
            CheckIndexCell(indexCell1);
            CheckIndexCell(indexCell2);

            return _adjacencMatrix[indexCell1, indexCell2] != 0;
        }
    }
}
