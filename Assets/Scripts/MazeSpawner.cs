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

            for(int x = 0; x < maze.width; x++)
            {
                for (int y = 0; y < maze.height; y++)
                {
                    gameObjectsMazeCells[x,y] = Object.Instantiate(cellPrefab, new Vector2((x - centerCell.x)*sizeCell.x, (y - centerCell.y) * sizeCell.y), Quaternion.identity);
                    gameObjectsMazeCells[x, y].transform.localScale = new Vector3(sizeCell.x, sizeCell.y, gameObjectsMazeCells[x, y].transform.localScale.z);

                    var cell = gameObjectsMazeCells[x, y].GetComponent<GameObjectMazeCell>();
                    cell.size = sizeCell;

                    var walls = maze.cells[x, y].walls;
                    for (int i = 0; i<cell.walls.Length; i++)
                    {
                        if(walls % 2 == 0)
                        {
                            cell.walls[i].SetActive(false);
                        }
                        walls >>= 1;
                    }
                }
            }
        }
    }
}
