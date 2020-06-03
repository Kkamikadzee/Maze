using System;
using Maze.OrthogonalMaze;
using Maze.RecursiveBacktracker;
using Maze.ThetaMaze;

using UnityEngine;

namespace Maze
{
    public class MazeManager
    {
        private TessellationType _tessellation;
        private GenerationAlgorithmEnum _generationAlgorithm;
        private Maze _maze;
        private MazeGenerator _mazeGenerator;
        private MazeSpawner _mazeSpawner;

        public MazeManager(TessellationType tessellationType, GenerationAlgorithmEnum generationAlgorithm)
        {
            ChangeTessellationType(tessellationType);
            ChangeGenerationAlgorithm(generationAlgorithm);
        }

        public void ChangeTessellationType(TessellationType tessellationType)
        {
            if (!Enum.IsDefined(typeof(TessellationType), tessellationType))
            {
                throw new Exception("Tessellation type is not found");
            }
            _tessellation = tessellationType;
        }
    
        public void ChangeGenerationAlgorithm(GenerationAlgorithmEnum generationAlgorithm)
        {
            switch (generationAlgorithm)
            {
                case GenerationAlgorithmEnum.RecursiveBacktracker:
                {
                    _mazeGenerator = new RecursiveBacktrackerMazeGenerator();
                }
                    break;
                default:
                    throw new Exception("Generation algorithm is not found");
            }
            _generationAlgorithm = generationAlgorithm;
        }

        public void CreateMaze(Vector3 positionMaze, float sizeMaze, params int[] args)
        {        
            switch (_tessellation)
            {
                case TessellationType.Orthigonal:
                    _mazeSpawner = new OrthogonalMazeSpawner(sizeMaze, positionMaze);
                    break;
                case TessellationType.Theta:
                    _mazeSpawner = new ThetaMazeSpawner(sizeMaze, positionMaze);
                    break;
            }

            _maze = _mazeGenerator.GenerateMaze(_tessellation, args);

            _mazeSpawner.SpawnMazeOnScene(_maze);
        }

        public void DestroyMaze()
        {
            _mazeSpawner.DestroyMaze();
        }
    }
}
