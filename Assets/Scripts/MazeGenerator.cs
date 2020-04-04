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
            Maze maze = new Maze();
            maze.InitializeOrthogonalMaze(Width, Height, positionStart);
            
            RemoveWallsRecursiveBacktracker(maze);

            PlaceMazeFinish(maze);

            return maze;
        }

        private void PlaceMazeFinish(Maze maze)
        {
            MazeCell furthest = maze.Cells[0];

            for (int i = 1; i < maze.AmountCells; i++)
            {
                if (maze.Cells[i].DistanceFromStart > furthest.DistanceFromStart)
                {
                    furthest = maze.Cells[i];
                }
            }

            maze.PositionFinish = furthest.position;
        }

        private void RemoveWallsRecursiveBacktracker(Maze maze)
        {
            int indexCurrentCell = maze.IndexStartCell;

            System.Collections.BitArray visitedCell = new System.Collections.BitArray(maze.AmountCells);

            Stack<int> stackCells = new Stack<int>();

            do
            {
                visitedCell[indexCurrentCell] = true;

                List<int> unvisitedNeighbours = new List<int>(maze.Cells[indexCurrentCell].AmountNeighbours);

                for(int i = 0; i<maze.AmountCells; i++)
                {
                    if(maze.AdjacencMatrix[indexCurrentCell, i] > 0)
                    {
                        if(!visitedCell[i])
                        {
                            unvisitedNeighbours.Add(i);
                        }
                    }
                    if(unvisitedNeighbours.Count == maze.Cells[indexCurrentCell].AmountNeighbours)
                    {
                        break;
                    }
                }

                if (unvisitedNeighbours.Count > 0)
                {
                    int indexChosenCell = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                    maze.AdjacencMatrix[indexCurrentCell, indexChosenCell] = 0; 
                    maze.AdjacencMatrix[indexChosenCell, indexCurrentCell] = 0;
                    stackCells.Push(indexCurrentCell);
                    maze.Cells[indexChosenCell].DistanceFromStart = maze.Cells[indexCurrentCell].DistanceFromStart + 1;
                    indexCurrentCell = indexChosenCell;
                }
                else
                {
                    indexCurrentCell = stackCells.Pop();
                }

            } while (stackCells.Count > 0);
        }
    }
}
