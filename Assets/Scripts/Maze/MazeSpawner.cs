using UnityEngine;

namespace Maze
{
    public abstract class MazeSpawner
    {
        protected GameObject _mazeObject;
        protected GameObject[] _gameObjectsMazeCells;
        protected float _sizeCell;
        protected Vector3 _positionMaze;

        protected MazeSpawner(float sizeCell, Vector3 positionMaze)
        {
            _sizeCell = sizeCell;
            _positionMaze = positionMaze;
        }

        public abstract void SpawnMazeOnScene(Maze maze);

        public void DestroyMaze()
        {
            foreach (var gameObject in _gameObjectsMazeCells)
            {
                Object.Destroy(gameObject);
            }
            Object.Destroy(_mazeObject);
        }
    }
}