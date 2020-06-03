using System;

namespace Maze.RecursiveBacktracker
{
    public class RecursiveBacktrackerMazeGenerator : MazeGenerator
    {
        private RecursiveBacktracker _recursiveBackTracker;

        public RecursiveBacktrackerMazeGenerator()
        {
            _recursiveBackTracker = new RecursiveBacktracker();
        }

        protected override OrthogonalMaze.OrthogonalMaze GenerateOrthogonalMaze(int width, int height)
        {
            OrthogonalMaze.OrthogonalMaze maze = new OrthogonalMaze.OrthogonalMaze(width, height);
            maze.InitializeAdjacencMatrix();

            _recursiveBackTracker.RemoveWalls(maze);

            return maze;
        }

        protected override ThetaMaze.ThetaMaze GenerateThetaMaze(int outerDiameter, int innerDiameter, int amountCellInFirstLayer,
            bool bias)
        {
            ThetaMaze.ThetaMaze maze = new ThetaMaze.ThetaMaze(outerDiameter, innerDiameter, amountCellInFirstLayer, bias);
            maze.InitializeAdjacencMatrix();

            _recursiveBackTracker.RemoveWalls(maze);

            return maze;
        }

        public override Maze GenerateMaze(TessellationType tessellationType, params int[] args)
        {
            switch (tessellationType)
            {
                case TessellationType.Orthigonal:
                    return GenerateOrthogonalMaze(args[0], args[1]);
                case TessellationType.Theta:
                    return GenerateThetaMaze(args[0], args[1], args[2], Convert.ToBoolean(args[3]));
            }

            throw new Exception("Unknown tessellation type");
        }
    }
}