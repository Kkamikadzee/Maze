using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maze.RecursiveBacktracker
{
    public class RecursiveBacktracker
    {
        private int _indexCurrentCell;
        private int _indexChosenCell;
        private BitArray _visitedCell;
        private Stack<int> _cellIndexesStack;
        private List<(int, byte)> _unvisitedNeighbours;

        private void FillListUnvisitedNeighbours(Maze maze)
        {
            _unvisitedNeighbours = maze.GetIndicesNeighboursCell(_indexCurrentCell);

            for (int i = _unvisitedNeighbours.Count - 1; i >= 0; i--)
            {
                if (_visitedCell[_unvisitedNeighbours[i].Item1])
                {
                    _unvisitedNeighbours.RemoveAt(i);
                }
            }
        }

        private int GetRandomUnvisitedNeighbour()
        {
            return _unvisitedNeighbours[
                UnityEngine.Random.Range(0, _unvisitedNeighbours.Count)].Item1; //TODO: Учитывать вес стены
        }

        private void Clear()
        {
            _visitedCell = null;
            _cellIndexesStack = null;
            _unvisitedNeighbours = null;
        }
    
        public void RemoveWalls(Maze maze)
        {
            _indexCurrentCell = maze.IndexStartCell;

            _visitedCell = new System.Collections.BitArray(maze.AmountCells);

            _cellIndexesStack = new Stack<int>();

            do
            {
                _visitedCell[_indexCurrentCell] = true;
                
                FillListUnvisitedNeighbours(maze);

                if (_unvisitedNeighbours.Count > 0)
                {
                    _indexChosenCell = GetRandomUnvisitedNeighbour();
                    maze.UnlinkCells(_indexCurrentCell, _indexChosenCell);
                    _cellIndexesStack.Push(_indexCurrentCell);
                    maze.SetDistanceFromStartForCell(_indexChosenCell,
                        maze.GetDistanceFromStartForCell(_indexCurrentCell) + 1u);
                    _indexCurrentCell = _indexChosenCell;
                }
                else
                {
                    _indexCurrentCell = _cellIndexesStack.Pop();
                }
            } while (_cellIndexesStack.Count > 0);
        
            Clear();
        }
    }
}
