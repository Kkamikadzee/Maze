using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MazeGenerator
{
    public Maze GenerateOrthogonalMaze(int width, int height, Vector2Int positionStart)
    {
        Maze maze = new Maze();
        maze.InitializeOrthogonalMaze(width, height, positionStart);
            
        RemoveWallsRecursiveBacktracker(maze);

        PlaceMazeFinish(maze);

        return maze;
    }

    public Maze GenerateThetaMaze(int outerDiameter, int innerDiameter, int firstLayer, bool bias)
    {
        Maze maze = new Maze();
        maze.InitializeThetaMaze(outerDiameter, firstLayer, bias);
            
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
                int indexChosenCell = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)]; //TODO: Учитывать вес стены
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