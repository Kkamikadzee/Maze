using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MazeGenerator
    {
        public Maze GenerateMaze(int Width, int Height, Vector2Int positionStart)
        {
            Maze maze = new Maze(Width, Height, positionStart);
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    maze.cells[x, y] = new MazeCell(new Vector2Int(x, y));
                }
            }
            
            RemoveWallsRecursiveBacktracker(maze);

            PlaceMazeFinish(maze);

            return maze;
        }

        private void PlaceMazeFinish(Maze maze)
        {
            MazeCell furthest = maze.cells[0, 0];

            for (int x = 0; x < maze.width; x++)
            {
                if (maze.cells[x, 0].DistanceFromStart > furthest.DistanceFromStart)
                {
                    furthest = maze.cells[x, 0];
                }
                if (maze.cells[x, maze.height - 1].DistanceFromStart > furthest.DistanceFromStart)
                {
                    furthest = maze.cells[x, maze.height - 1];
                }
            }

            for (int y = 0; y < maze.height; y++)
            {
                if (maze.cells[0, y].DistanceFromStart > furthest.DistanceFromStart)
                {
                    furthest = maze.cells[0, y];
                }
                if (maze.cells[maze.width - 1, y].DistanceFromStart > furthest.DistanceFromStart)
                {
                    furthest = maze.cells[maze.width - 1, y];
                }
            }

            maze.positionFinish = furthest.position;
        }

        private void RemoveWallsRecursiveBacktracker(Maze maze)
        {
            int width = maze.cells.GetLength(0);
            int height = maze.cells.GetLength(1);

            MazeCell currentCell = maze.StartCell;

            currentCell.Visited = true;
            Stack<MazeCell> stackCells = new Stack<MazeCell>();
            do
            {
                List<MazeCell> unvisitedNeighbours = new List<MazeCell>();

                int x = currentCell.position.x;
                int y = currentCell.position.y;

                if (x > 0 && !maze.cells[x - 1, y].Visited)
                {
                    unvisitedNeighbours.Add(maze.cells[x - 1, y]);
                }
                if (x < width - 1 && !maze.cells[x + 1, y].Visited)
                {
                    unvisitedNeighbours.Add(maze.cells[x + 1, y]);
                }
                if (y > 0 && !maze.cells[x, y - 1].Visited)
                {
                    unvisitedNeighbours.Add(maze.cells[x, y - 1]);
                }
                if (y < height - 1 && !maze.cells[x, y + 1].Visited)
                {
                    unvisitedNeighbours.Add(maze.cells[x, y + 1]);
                }

                if (unvisitedNeighbours.Count > 0)
                {
                    MazeCell chosenCell = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                    RemoveWall(currentCell, chosenCell);
                    chosenCell.Visited = true;
                    stackCells.Push(currentCell);
                    chosenCell.DistanceFromStart = currentCell.DistanceFromStart + 1;
                    currentCell = chosenCell;
                }
                else
                {
                    currentCell = stackCells.Pop();
                }

            } while (stackCells.Count > 0);
        }

        private void RemoveWall(MazeCell currentCell, MazeCell chosenCell)
        {
            if (currentCell.position.x == chosenCell.position.x)
            {
                if(currentCell.position.y > chosenCell.position.y)
                {
                    currentCell.walls ^= 0b1000;
                    chosenCell.walls ^= 0b0100;
                }
                else
                {
                    currentCell.walls ^= 0b0100;
                    chosenCell.walls ^= 0b1000;
                }
            }
            else
            {
                if (currentCell.position.x > chosenCell.position.x)
                {
                    currentCell.walls ^= 0b0001;
                    chosenCell.walls ^= 0b0010;
                }
                else
                {
                    currentCell.walls ^= 0b0010;
                    chosenCell.walls ^= 0b0001;
                }
            }
        }
    }
}
