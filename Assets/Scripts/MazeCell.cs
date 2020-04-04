using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MazeCell
    {
        public Vector2 position;
        public uint DistanceFromStart;

        public int amountNeighbours;

        public int AmountNeighbours => amountNeighbours;

        public MazeCell(Vector2 position, int amountNeighbours)
        {
            this.position = position;
            this.amountNeighbours = amountNeighbours;
        }
        public static bool operator ==(MazeCell cell1, MazeCell cell2)
        {
            if (cell1.position == cell2.position)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(MazeCell cell1, MazeCell cell2)
        {
            if (cell1.position != cell2.position)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is MazeCell cell &&
                   position.Equals(cell.position) &&
                   DistanceFromStart == cell.DistanceFromStart;
        }
        public override int GetHashCode()
        {
            int hashCode = -881634862;
            hashCode = hashCode * -1521134295 + position.GetHashCode();
            hashCode = hashCode * -1521134295 + DistanceFromStart.GetHashCode();
            return hashCode;
        }
    }
}
