using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner
{
    private GameObject mazeObject;

    public GameObject cellPrefab;
    public Maze maze;
    public GameObject[] gameObjectsMazeCells;
    
    MazeGenerator generator;

    public Vector2 CoordinateStart => maze.PositionStart;
    public MazeSpawner(GameObject cellPrefab)
    {
        this.cellPrefab = cellPrefab;
        generator = new MazeGenerator();
    }
    public void SpawnOrthogonalMaze(int width, int height, Vector2 sizeCell)
    {
        mazeObject = new GameObject("Maze");
        var mazeTransform = mazeObject.GetComponent<Transform>();
        
        maze = generator.GenerateOrthogonalMaze(width, height, Vector2Int.zero);
        gameObjectsMazeCells = new GameObject[width * height];

        Vector2Int centerCell = new Vector2Int 
        {
            x = width/2, 
            y = height /2 
        };

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gameObjectsMazeCells[(x) + width * (y)] = UnityEngine.Object.Instantiate(cellPrefab, new Vector2((x - centerCell.x)*sizeCell.x, (y - centerCell.y) * sizeCell.y), Quaternion.identity, mazeTransform);
                gameObjectsMazeCells[(x) + width * (y)].transform.localScale = new Vector3(sizeCell.x, sizeCell.y, gameObjectsMazeCells[(x) + width * (y)].transform.localScale.z);

                var cell = gameObjectsMazeCells[(x) + width * (y)].GetComponent<GameObjectMazeCell>();
                cell.size = sizeCell;
            }
        }

        DisableOrthogonalWalls(width, height);
    }

    private void DisableOrthogonalWalls(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var currentCell = gameObjectsMazeCells[(x) + width * (y)].GetComponent<GameObjectMazeCell>();
                if (x - 1 >= 0)
                {
                    if(maze.AdjacencMatrix[(x) + width * (y), (x - 1) + width * (y)] == 0)
                    {
                        var chosenCell = gameObjectsMazeCells[(x - 1) + width * (y)].GetComponent<GameObjectMazeCell>();
                        currentCell.walls[0].SetActive(false);
                        chosenCell.walls[1].SetActive(false);
                    }
                }
                if (x + 1 < width)
                {
                    if(maze.AdjacencMatrix[(x) + width * (y), (x + 1) + width * (y)] == 0)
                    {
                        var chosenCell = gameObjectsMazeCells[(x + 1) + width * (y)].GetComponent<GameObjectMazeCell>();
                        currentCell.walls[1].SetActive(false);
                        chosenCell.walls[0].SetActive(false);
                    }

                }
                if (y - 1 >= 0)
                {
                    if (maze.AdjacencMatrix[(x) + width * (y), (x) + width * (y - 1)] == 0)
                    {
                        var chosenCell = gameObjectsMazeCells[(x) + width * (y - 1)].GetComponent<GameObjectMazeCell>();
                        currentCell.walls[3].SetActive(false);
                        chosenCell.walls[2].SetActive(false);

                    }
                }
                if (y + 1 < height)
                {
                    if (maze.AdjacencMatrix[(x) + width * (y), (x) + width * (y + 1)] == 0)
                    {
                        var chosenCell = gameObjectsMazeCells[(x) + width * (y + 1)].GetComponent<GameObjectMazeCell>();
                        currentCell.walls[2].SetActive(false);
                        chosenCell.walls[3].SetActive(false);
                    }
                }
            }
        }
    }
    
    public void SpawnThetaMaze(int outerDiameter, int innerDiameter, int firstLayer, bool bias = false)
    {
        mazeObject = new GameObject("Maze");
        var mazeTransform = mazeObject.transform;

        maze = generator.GenerateThetaMaze(outerDiameter, innerDiameter, firstLayer, bias);
        
        int amountCells = 1 + firstLayer * (outerDiameter - 1) + (outerDiameter - 2) * (outerDiameter - 1) * 2; //Кол-во ячеек, если увеличивать кол-во на 1 в каждой четверти на следующем слое
        gameObjectsMazeCells = new GameObject[amountCells];
        
        int amountCellsInPreviousLayers;
        int amountCellsInCurrentLayer;
        int amountCellsInNextLayer;
        
        Vector2 positionCell;
        float angleArc1;
        float angleArc2;
        float lastAngleArc;

        int neighbourArc, amountNeighbours;
        float angleArcNeighbour, angleArcInNextLayer;
        
        for (int currentLayer = 0; currentLayer < outerDiameter; currentLayer++)
        {
            amountCellsInPreviousLayers = currentLayer > 0 ? 1 + firstLayer * (currentLayer - 1) + (currentLayer - 2) * (currentLayer - 1) * 2 : 0;
            amountCellsInCurrentLayer = currentLayer > 0 ? firstLayer + (4 * (currentLayer - 1)) : 1;
            amountCellsInNextLayer = currentLayer > 0 ? firstLayer + (4 * currentLayer) : firstLayer;

            for (int currentCellInLayer = 0; currentCellInLayer < amountCellsInCurrentLayer; currentCellInLayer++)
            {
                var namCell = String.Format("Cell({0})", amountCellsInPreviousLayers + currentCellInLayer);

                angleArc1 = currentLayer > 0 ? (2 * Mathf.PI / amountCellsInCurrentLayer) * currentCellInLayer : 0;
                positionCell = new Vector2()
                         {
                             x = currentLayer * Mathf.Cos(angleArc1),
                             y = currentLayer * Mathf.Sin(angleArc1)
                         };

                gameObjectsMazeCells[amountCellsInPreviousLayers + currentCellInLayer] = new GameObject(namCell)
                {
                    transform =
                    {
                        position = positionCell, 
                        rotation = Quaternion.identity, 
                        parent = mazeTransform
                    }
                };
                var cell = gameObjectsMazeCells[amountCellsInPreviousLayers + currentCellInLayer];
                
                angleArc2 = (2 * Mathf.PI / amountCellsInCurrentLayer) * (currentCellInLayer + 1);
                angleArcInNextLayer = 2 * Mathf.PI / amountCellsInNextLayer;

                neighbourArc =
                    Mathf.RoundToInt(
                        angleArc1 / angleArcInNextLayer);
                amountNeighbours = 0;
                angleArcNeighbour = angleArcInNextLayer * neighbourArc;
                GameObject currentWall;
                LineRenderer line;
                while (angleArcNeighbour < angleArc2)
                {
                    currentWall = new GameObject(String.Format("Wall({0})", amountNeighbours))
                    {
                        transform =
                        {
                            position = Vector3.zero,
                            parent =  cell.transform
                        }
                    };
                    
                    line = currentWall.AddComponent<LineRenderer>();
                    line.useWorldSpace = false;
                    line.widthMultiplier = 0.16f;
                    line.numCapVertices = 4;

                    var collider = currentWall.AddComponent<EdgeCollider2D>();

                    lastAngleArc = amountNeighbours == 0 ? angleArc1 : angleArcNeighbour;
                    amountNeighbours++;
                    angleArcNeighbour = angleArcInNextLayer * (neighbourArc + amountNeighbours);
                    angleArcNeighbour = angleArcNeighbour > angleArc2 ? angleArc2 : angleArcNeighbour;
                    var positionsArc = getPositionsArc(currentLayer, lastAngleArc, angleArcNeighbour, 32);
                    line.positionCount = positionsArc.Length;
                    line.SetPositions(positionsArc);
                    collider.points = ConvetrToVector2(positionsArc);
                }
                if (currentLayer != 0)
                {
                    currentWall = new GameObject(String.Format("Wall({0})", amountNeighbours))
                    {
                        transform =
                        {
                            position = Vector3.zero,
                            parent =  cell.transform
                        }
                    };
                    line = currentWall.AddComponent<LineRenderer>();
                    line.useWorldSpace = false;
                    line.widthMultiplier = 0.16f;
                    line.numCapVertices = 4;

                    var collider = currentWall.AddComponent<EdgeCollider2D>();

                    var positionsArc = getPositions(currentLayer, angleArc1);
                    line.positionCount = positionsArc.Length;
                    line.SetPositions(positionsArc);
                    collider.points = ConvetrToVector2(positionsArc);
                }
            }
        }

        // DisableOrthogonalWalls(width, height);
    }

    private Vector2[] ConvetrToVector2(Vector3[] vector3Array)
    {
        Vector2 []res = new Vector2[vector3Array.Length];
        for (var i = 0; i < vector3Array.Length; i++)
        {
            res[i] = (Vector2)(vector3Array[i]);
        }

        return res;
    }

    private Vector3[] getPositions(int currentLayer, float angleArc)
    {
        Vector3 []positions = new Vector3[2];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector3()
            {
                x = (currentLayer + i) * Mathf.Cos(angleArc),
                y = (currentLayer + i) * Mathf.Sin(angleArc)
            };
        }
        
        return positions;
    }

    private Vector3[] getPositionsArc(int currentLayer, float angleArc, float angleArcNeighbour, int countArcInCircle)
    {
        var delta = 2 * Mathf.PI / countArcInCircle;
        var countPositions = Mathf.CeilToInt((angleArcNeighbour - angleArc) / delta) + 1; // +1 ибо может быть на 1 отрезок больше
        List<Vector3> positions = new List<Vector3>(countPositions);
        float phi;
        for (phi = angleArc; phi < angleArcNeighbour; phi += delta)
        {
            positions.Add(new Vector3()
            {
                x = (currentLayer + 1) * Mathf.Cos(phi),
                y = (currentLayer + 1) * Mathf.Sin(phi),
                z = 0
            });
        }
        positions.Add(new Vector3()
        {
            x = (currentLayer + 1) * Mathf.Cos(angleArcNeighbour),
            y = (currentLayer + 1) * Mathf.Sin(angleArcNeighbour),
            z = 0
        });

        return positions.ToArray();
    }
}