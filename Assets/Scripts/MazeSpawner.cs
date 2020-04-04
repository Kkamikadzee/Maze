using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MazeSpawner
    {
        public GameObject cellPrefab;
        public Maze maze;
        public GameObject[,] gameObjectsMazeCells;

        MazeGenerator generator;
        public MazeSpawner(GameObject cellPrefab)
        {
            this.cellPrefab = cellPrefab;
            generator = new MazeGenerator();
        }
        public void SpawnMaze(int width, int height, Vector2 sizeCell)
        {
            maze = generator.GenerateMaze(width, height, Vector2Int.zero);
            gameObjectsMazeCells = new GameObject[width, height];

            Vector2Int centerCell = new Vector2Int 
            {
                x = width/2, 
                y = height /2 
            };

            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gameObjectsMazeCells[x,y] = UnityEngine.Object.Instantiate(cellPrefab, new Vector2((x - centerCell.x)*sizeCell.x, (y - centerCell.y) * sizeCell.y), Quaternion.identity);
                    gameObjectsMazeCells[x, y].transform.localScale = new Vector3(sizeCell.x, sizeCell.y, gameObjectsMazeCells[x, y].transform.localScale.z);

                    var cell = gameObjectsMazeCells[x, y].GetComponent<GameObjectMazeCell>();
                    cell.size = sizeCell;
                }
            }

            DisableWalls(width, height);
        }

        private void DisableWalls(int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var currentCell = gameObjectsMazeCells[x, y].GetComponent<GameObjectMazeCell>();
                    if (x - 1 >= 0)
                    {
                        if(maze.AdjacencMatrix[(x) + width * (y), (x - 1) + width * (y)] == 0)
                        {
                            var chosenCell = gameObjectsMazeCells[x - 1, y].GetComponent<GameObjectMazeCell>();
                            currentCell.walls[0].SetActive(false);
                            chosenCell.walls[1].SetActive(false);
                        }
                    }
                    if (x + 1 < width)
                    {
                        if(maze.AdjacencMatrix[(x) + width * (y), (x + 1) + width * (y)] == 0)
                        {
                            var chosenCell = gameObjectsMazeCells[x + 1, y].GetComponent<GameObjectMazeCell>();
                            currentCell.walls[1].SetActive(false);
                            chosenCell.walls[0].SetActive(false);
                        }

                    }
                    if (y - 1 >= 0)
                    {
                        if (maze.AdjacencMatrix[(x) + width * (y), (x) + width * (y - 1)] == 0)
                        {
                            var chosenCell = gameObjectsMazeCells[x, y - 1].GetComponent<GameObjectMazeCell>();
                            currentCell.walls[3].SetActive(false);
                            chosenCell.walls[2].SetActive(false);

                        }
                    }
                    if (y + 1 < height)
                    {
                        if (maze.AdjacencMatrix[(x) + width * (y), (x) + width * (y + 1)] == 0)
                        {
                            var chosenCell = gameObjectsMazeCells[x, y + 1].GetComponent<GameObjectMazeCell>();
                            currentCell.walls[2].SetActive(false);
                            chosenCell.walls[3].SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
