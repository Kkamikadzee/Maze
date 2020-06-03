namespace Maze
{
    public abstract class MazeGenerator
    {
        protected GenerationAlgorithmEnum _generationAlgorithm;

        protected abstract OrthogonalMaze.OrthogonalMaze GenerateOrthogonalMaze(int width, int height);

        protected abstract ThetaMaze.ThetaMaze GenerateThetaMaze(int outerDiameter, int innerDiameter, int amountCellInFirstLayer,
            bool bias);

        public abstract Maze GenerateMaze(TessellationType tessellationType, params int[] args);
    }
}
