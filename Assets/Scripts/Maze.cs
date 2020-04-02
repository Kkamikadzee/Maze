using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MazeCell
    {
        public Vector2Int position;
        public ushort walls; //До 16 стен можно хранить
        public uint DistanceFromStart;

        public bool Visited = false;

        public MazeCell(Vector2Int position)
        {
            this.position = position;
            walls = ushort.MaxValue;
        }
    }
    public class Maze
    {
        public int width;
        public int height;

        public MazeCell[,] cells;
        public Vector2Int positionStart;
        public Vector2Int positionFinish;

        public MazeCell StartCell => cells[positionStart.x, positionStart.y];

        public Maze(int Width, int Height, Vector2Int positionStart)
        {
            this.width = Width;
            this.height = Height;
            cells = new MazeCell[Width, Height];
            this.positionStart = positionStart;
        }
    }
}
