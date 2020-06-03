using UnityEngine;
#pragma warning disable 660,661

namespace Maze
{
    public class MazeCell
    {
        private readonly Vector2 _position;
        private uint _distanceFromStart;

        private readonly int _amountNeighbours;

        public int AmountNeighbours => _amountNeighbours;
    
        public uint DistanceFromStart
        {
            get => _distanceFromStart;
            set => _distanceFromStart = value;
        }

        public MazeCell(Vector2 position, int amountNeighbours)
        {
            _position = position;
            _amountNeighbours = amountNeighbours;
        }
        public static bool operator ==(MazeCell cell1, MazeCell cell2)
        {
            if ((cell1 is null) || (cell2 is null))
            {
                return false;
            }
            return (cell1._position == cell2._position) && 
                   (cell1._amountNeighbours == cell2._amountNeighbours) &&
                   (cell1._distanceFromStart == cell2._distanceFromStart);
        }
        public static bool operator !=(MazeCell cell1, MazeCell cell2)
        {
            if ((cell1 is null) || (cell2 is null))
            {
                return true;
            }
            return (cell1._position != cell2._position) || 
                   (cell1._amountNeighbours != cell2._amountNeighbours) ||
                   (cell1._distanceFromStart != cell2._distanceFromStart);
        }
    }
}