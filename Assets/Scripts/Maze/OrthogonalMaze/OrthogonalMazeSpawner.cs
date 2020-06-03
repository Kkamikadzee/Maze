using System;
using UnityEngine;

namespace Maze.OrthogonalMaze
{
    public class OrthogonalMazeSpawner : MazeSpawner
    {
        public OrthogonalMazeSpawner(float sizeCell, Vector3 positionMaze) : base(sizeCell, positionMaze)
        {
        }

        private bool EnableOrthogonalWalls(OrthogonalMaze maze, int x, int y, int numberWall)
        {
            switch (numberWall)
            {
                case 0:
                {
                    if ((y == 0))
                    {
                        return true;
                    }
                    return (maze.ThereIsWall(x + maze.Width * y, x + maze.Width * (y - 1)));
                }
                case 1:
                {
                    if ((y == maze.Height - 1))
                    {
                        return true;
                    }
                    return (maze.ThereIsWall(x + maze.Width * y, x + maze.Width * (y + 1)));
                }
                case 2:
                {
                    if ((x == 0))
                    {
                        return true;
                    }
                    return (maze.ThereIsWall(x + maze.Width * y, x - 1 + maze.Width * y));
                }
                case 3:
                {
                    if ((x == maze.Width - 1))
                    {
                        return true;
                    }
                    return (maze.ThereIsWall(x + maze.Width * y, x + 1 + maze.Width * y));
                }
                default:
                    return false;
            }
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
    
        private Vector3[][] GetPositionsWalls(Vector2 positionCell)
        {
            var positionsWalls = new Vector3[4][]
            {
                new Vector3[2]
                {
                    new Vector3(positionCell.x + _sizeCell, positionCell.y),
                    new Vector3(positionCell.x, positionCell.y),
                },
                new Vector3[2]
                {
                    new Vector3(positionCell.x + _sizeCell, positionCell.y + _sizeCell),
                    new Vector3(positionCell.x, positionCell.y + _sizeCell),
                },
                new Vector3[2]
                {
                    new Vector3(positionCell.x, positionCell.y + _sizeCell),
                    new Vector3(positionCell.x, positionCell.y),
                },
                new Vector3[2]
                {
                    new Vector3(positionCell.x + _sizeCell, positionCell.y + _sizeCell),
                    new Vector3(positionCell.x + _sizeCell, positionCell.y),
                },
            };
            return positionsWalls;
        }

    
        public override void SpawnMazeOnScene(Maze inputMaze)
        {
            if (inputMaze is null)
            {
                throw new Exception("Input maze equals null");
            }
            else if (!(inputMaze is OrthogonalMaze))
            {
                throw new Exception("Input maze is not Theta maze");
            }

            var maze = inputMaze as OrthogonalMaze;
            _mazeObject = new GameObject("Maze");
            var mazeTransform = _mazeObject.transform;

            _gameObjectsMazeCells = new GameObject[maze.AmountCells];
            Vector2 leftBottomPoint = new Vector2
            {
                x = _positionMaze.x - (maze.Width * _sizeCell / 2f),
                y = _positionMaze.y - maze.Height * _sizeCell / 2f
            };

            Vector2 positionCell;
        
            for (int x = 0; x < maze.Width; x++)
            {
                for (int y = 0; y < maze.Height; y++)
                {
                    var nameCell = String.Format("Cell({0})", (x) + maze.Width * (y));

                    positionCell = new Vector2()
                    {
                        x = leftBottomPoint.x + (x*_sizeCell),
                        y = leftBottomPoint.y + (y*_sizeCell)
                    };
                
                    var cell = _gameObjectsMazeCells[(x) + maze.Width * (y)] = new GameObject(nameCell)
                    {
                        transform =
                        {
                            position = positionCell,
                            rotation = Quaternion.identity,
                            parent = mazeTransform
                        }
                    };

                    var positionsWalls = GetPositionsWalls(positionCell);

                    for (int i = 0; i < 4; i++)
                    {
                        if (!EnableOrthogonalWalls(maze, x, y, i))
                        {
                            continue;
                        }
                        var currentWall = new GameObject(String.Format("Wall({0})", i))
                        {
                            transform =
                            {
                                position = Vector3.zero,
                                parent = cell.transform
                            }
                        };

                        var line = currentWall.AddComponent<LineRenderer>();
                        line.useWorldSpace = false;
                        line.widthMultiplier = 0.16f * _sizeCell;
                        line.numCapVertices = 4;

                        var collider = currentWall.AddComponent<EdgeCollider2D>();

                        line.positionCount = 2;
                        line.SetPositions(positionsWalls[i]);
                        collider.points = ConvetrToVector2(positionsWalls[i]);
                    }
                }
            }
        }

    }
}