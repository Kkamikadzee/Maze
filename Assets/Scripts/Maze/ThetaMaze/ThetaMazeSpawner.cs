using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.ThetaMaze
{
    public class ThetaMazeSpawner : MazeSpawner
    {
        public ThetaMazeSpawner(float sizeCell, Vector3 positionMaze) : base(sizeCell, positionMaze)
        {
        }

        private bool EnableThetaWalls(ThetaMaze maze, int x, int y, int numberWall)
        {
            throw new Exception("KEK");
        }
    
        private Vector3[] GetPositions(int currentLayer, float angleArc)
        {
            Vector3[] positions = new Vector3[2];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector3()
                {
                    x = ((currentLayer + i) * Mathf.Cos(angleArc)) + _positionMaze.x,
                    y = ((currentLayer + i) * Mathf.Sin(angleArc)) + _positionMaze.y
                };
            }

            return positions;
        }
        private Vector3[] GetPositions(int currentLayer, float angleArc, float angleArcNeighbour, int countArcInCircle)
        {
            var delta = 2 * Mathf.PI / countArcInCircle;
            var countPositions =
                Mathf.CeilToInt((angleArcNeighbour - angleArc) / delta) + 1; // +1 ибо может быть на 1 отрезок больше
            List<Vector3> positions = new List<Vector3>(countPositions);
            float phi;
            for (phi = angleArc; phi < angleArcNeighbour; phi += delta)
            {
                positions.Add(new Vector3()
                {
                    x = ((currentLayer + 1) * Mathf.Cos(phi)) + _positionMaze.x,
                    y = ((currentLayer + 1) * Mathf.Sin(phi)) + _positionMaze.y,
                    z = _positionMaze.z
                });
            }

            positions.Add(new Vector3()
            {
                x = ((currentLayer + 1) * Mathf.Cos(angleArcNeighbour)) + _positionMaze.x,
                y = ((currentLayer + 1) * Mathf.Sin(angleArcNeighbour)) + _positionMaze.y,
                z = _positionMaze.z
            });

            return positions.ToArray();
        }
        private Vector2[] ConvetrToVector2(Vector3[] vector3Array)// TODO: Дублирование кода со второым классом
        {
            Vector2[] res = new Vector2[vector3Array.Length];
            for (var i = 0; i < vector3Array.Length; i++)
            {
                res[i] = (Vector2) (vector3Array[i]);
            }

            return res;
        }

        public override void SpawnMazeOnScene(Maze inputMaze) //TODO: Отключение ненужных стен
        {
            if (inputMaze is null)
            {
                throw new Exception("Input maze equals null");
            }
            else if (!(inputMaze is ThetaMaze))
            {
                throw new Exception("Input maze is not Theta maze");
            }

            var maze = inputMaze as ThetaMaze;
            _mazeObject = new GameObject("Maze");
            var mazeTransform = _mazeObject.transform;

            _gameObjectsMazeCells = new GameObject[maze.AmountCells];

            int amountCellsInPreviousLayers;
            int amountCellsInCurrentLayer;
            int amountCellsInNextLayer;

            Vector2 positionCell;
            float angleArc1;
            float angleArc2;
            float lastAngleArc;

            int neighbourArc, amountNeighbours;
            float angleArcNeighbour, angleArcInNextLayer;

            for (int currentLayer = 0;
                currentLayer < maze.OuterDiameter;
                currentLayer++) //Какой страшный цикл. Но разбивать я его не буду.
            {
                amountCellsInPreviousLayers = currentLayer > 0
                    ? 1 + maze.AmountCellInFirstLayer * (currentLayer - 1) + (currentLayer - 2) * (currentLayer - 1) * 2
                    : 0;
                amountCellsInCurrentLayer = currentLayer > 0 ? maze.AmountCellInFirstLayer + (4 * (currentLayer - 1)) : 1;
                amountCellsInNextLayer = currentLayer > 0
                    ? maze.AmountCellInFirstLayer + (4 * currentLayer)
                    : maze.AmountCellInFirstLayer;

                for (int currentCellInLayer = 0; currentCellInLayer < amountCellsInCurrentLayer; currentCellInLayer++)
                {
                    var nameCell = String.Format("Cell({0})", amountCellsInPreviousLayers + currentCellInLayer);

                    angleArc1 = currentLayer > 0 ? (2 * Mathf.PI / amountCellsInCurrentLayer) * currentCellInLayer : 0;
                    positionCell = new Vector2()
                    {
                        x = (currentLayer * Mathf.Cos(angleArc1)) + _positionMaze.x,
                        y = (currentLayer * Mathf.Sin(angleArc1)) + _positionMaze.y
                    };

                    _gameObjectsMazeCells[amountCellsInPreviousLayers + currentCellInLayer] = new GameObject(nameCell)
                    {
                        transform =
                        {
                            position = positionCell,
                            rotation = Quaternion.identity,
                            parent = mazeTransform
                        }
                    };
                    var cell = _gameObjectsMazeCells[amountCellsInPreviousLayers + currentCellInLayer];

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
                                parent = cell.transform
                            }
                        };

                        line = currentWall.AddComponent<LineRenderer>();
                        line.useWorldSpace = false;
                        line.widthMultiplier = 0.16f * _sizeCell;
                        line.numCapVertices = 4;

                        var collider = currentWall.AddComponent<EdgeCollider2D>();

                        lastAngleArc = amountNeighbours == 0 ? angleArc1 : angleArcNeighbour;
                        amountNeighbours++;
                        angleArcNeighbour = angleArcInNextLayer * (neighbourArc + amountNeighbours);
                        angleArcNeighbour = angleArcNeighbour > angleArc2 ? angleArc2 : angleArcNeighbour;
                        var positionsArc = GetPositions(currentLayer, lastAngleArc, angleArcNeighbour, 32);
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
                                parent = cell.transform
                            }
                        };
                        line = currentWall.AddComponent<LineRenderer>();
                        line.useWorldSpace = false;
                        line.widthMultiplier = 0.16f;
                        line.numCapVertices = 4;

                        var collider = currentWall.AddComponent<EdgeCollider2D>();

                        var positionsArc = GetPositions(currentLayer, angleArc1);
                        line.positionCount = positionsArc.Length;
                        line.SetPositions(positionsArc);
                        collider.points = ConvetrToVector2(positionsArc);
                    }
                }
            }
        }
    }
}