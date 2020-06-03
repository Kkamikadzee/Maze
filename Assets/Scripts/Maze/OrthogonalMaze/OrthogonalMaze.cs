using UnityEngine;

namespace Maze.OrthogonalMaze
{
    public class OrthogonalMaze : Maze
    {
        private readonly int _width;
        private readonly int _height;

        public int Width => _width;
        public int Height => _height;

        public OrthogonalMaze(int width, int height)
        {
            _width = width;
            _height = height;

            _cells = new MazeCell[_width * _height];
            _adjacencMatrix = new byte[AmountCells, AmountCells];
        }

        public override void InitializeAdjacencMatrix()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (x - 1 >= 0)
                    {
                    
                        _adjacencMatrix[(x) + _width * (y), (x - 1) + _width * (y)] = 1;
                        _adjacencMatrix[(x - 1) + _width * (y), (x) + _width * (y)] = 1;
                    }

                    if (x + 1 < _width)
                    {
                        _adjacencMatrix[(x) + _width * (y), (x + 1) + _width * (y)] = 1;
                        _adjacencMatrix[(x + 1) + _width * (y), (x) + _width * (y)] = 1;
                    }

                    if (y - 1 >= 0)
                    {
                        _adjacencMatrix[(x) + _width * (y), (x) + _width * (y - 1)] = 1;
                        _adjacencMatrix[(x) + _width * (y - 1), (x) + _width * (y)] = 1;
                    }

                    if (y + 1 < _height)
                    {
                        _adjacencMatrix[(x) + _width * (y), (x) + _width * (y + 1)] = 1;
                        _adjacencMatrix[(x) + _width * (y + 1), (x) + _width * (y)] = 1;
                    }

                    _cells[x + _width * y] = new MazeCell(new Vector2(x, y), 4);
                }
            }
        }
    }
}