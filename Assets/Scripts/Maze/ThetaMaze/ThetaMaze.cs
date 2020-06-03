using UnityEngine;

namespace Maze.ThetaMaze
{
    public class ThetaMaze : Maze
    {
        private readonly int _outerDiameter;
        private readonly int _innerDiameter;
        private readonly int _amountCellInFirstLayer;
        private readonly bool _bias;

        public int OuterDiameter => _outerDiameter;

        public int InnerDiameter => _innerDiameter;

        public int AmountCellInFirstLayer => _amountCellInFirstLayer;

        public bool Bias => _bias;

        public ThetaMaze(int outerDiameter, int innerDiameter, int amountCellInFirstLayer, bool bias)
        {
            _outerDiameter = outerDiameter;
            _innerDiameter = innerDiameter;
            _amountCellInFirstLayer = amountCellInFirstLayer;
            _bias = bias;
        
            //Кол-во ячеек, если увеличивать кол-во на 1 в каждой четверти на следующем слое
            int amountCells =
                1 + _amountCellInFirstLayer * (outerDiameter - 1) +
                (outerDiameter - 2) * (outerDiameter - 1) * 2;
            _cells = new MazeCell[amountCells];
            _adjacencMatrix = new byte[amountCells, amountCells];

        }

        public override void InitializeAdjacencMatrix()
        {
            byte weightSideWalls; //Вес боковых стен
            byte weightCircularWalls; //Вес стен на сосдних слоях

            if (_bias)
            {
                weightSideWalls = 1;
                weightCircularWalls = 2;
            }
            else
            {
                weightSideWalls = 1;
                weightCircularWalls = 1;
            }

            //Центральная ячейка
            if (_innerDiameter == 1)
            {
                for (int i = 1; i <= _amountCellInFirstLayer; i++)
                {
                    _adjacencMatrix[0, i] = weightCircularWalls;
                    _adjacencMatrix[i, 0] = weightCircularWalls;
                }
            }

            _cells[0] = new MazeCell(Vector2.zero, _amountCellInFirstLayer);
        
            //Остальные
            int amountCellsInPreviousLayers, amountCellsInCurrentLayer, amountCellsInNextLayer, previousCellInLayer, 
                nextCellInLayer, neighbourArc, amountNeighbours;
            float angleArc1, angleArc2, angleArcInNextLayer, angleArcNeighbour;
            for (int currentLayer = _innerDiameter - 1; currentLayer < _outerDiameter - 1; currentLayer++)
            {
                amountCellsInPreviousLayers = 1 + _amountCellInFirstLayer * (currentLayer) + (currentLayer - 1) * (currentLayer) * 2;
                amountCellsInCurrentLayer = _amountCellInFirstLayer + (4 * currentLayer);
                amountCellsInNextLayer = _amountCellInFirstLayer + (4 * (currentLayer + 1));
                for (int currentCellInLayer = 0; currentCellInLayer < amountCellsInCurrentLayer; currentCellInLayer++)
                {
                    previousCellInLayer =
                        currentCellInLayer > 0 ? currentCellInLayer - 1 : amountCellsInCurrentLayer - 1;
                    nextCellInLayer = (currentCellInLayer + 1) % amountCellsInCurrentLayer;
                    //Соседние ячейки на одном слое
                    _adjacencMatrix[amountCellsInPreviousLayers + currentCellInLayer,
                        amountCellsInPreviousLayers + previousCellInLayer] = weightSideWalls;
                    _adjacencMatrix[amountCellsInPreviousLayers + previousCellInLayer,
                        amountCellsInPreviousLayers + currentCellInLayer] = weightSideWalls;
                    _adjacencMatrix[amountCellsInPreviousLayers + currentCellInLayer,
                        amountCellsInPreviousLayers + nextCellInLayer] = weightSideWalls;
                    _adjacencMatrix[amountCellsInPreviousLayers + nextCellInLayer,
                        amountCellsInPreviousLayers + currentCellInLayer] = weightSideWalls;

                    angleArc1 = (2 * Mathf.PI / amountCellsInCurrentLayer) * currentCellInLayer;

                    angleArc2 = (2 * Mathf.PI / amountCellsInCurrentLayer) * (currentCellInLayer + 1);
                    angleArcInNextLayer = 2 * Mathf.PI / amountCellsInNextLayer;

                    neighbourArc =
                        Mathf.RoundToInt(
                            angleArc1 / angleArcInNextLayer);
                    amountNeighbours = 0;
                    angleArcNeighbour = angleArcInNextLayer * neighbourArc;
                    while (angleArcNeighbour < angleArc2)
                    {
                        if (currentLayer < _outerDiameter - 2
                        ) // -2, ибо -1, чтобы попасть на границу и ещё -1, ибо пропускаем центральную ячейку
                        {
                            _adjacencMatrix[amountCellsInPreviousLayers + currentCellInLayer,
                                    amountCellsInPreviousLayers + amountCellsInCurrentLayer + neighbourArc +
                                    amountNeighbours] =
                                weightCircularWalls;
                            _adjacencMatrix[
                                amountCellsInPreviousLayers + amountCellsInCurrentLayer + neighbourArc + amountNeighbours,
                                amountCellsInPreviousLayers + currentCellInLayer] = weightCircularWalls;
                        }

                        amountNeighbours++;
                        angleArcNeighbour = angleArcInNextLayer * (neighbourArc + amountNeighbours);
                    }

                    _cells[amountCellsInPreviousLayers + currentCellInLayer] =
                        new MazeCell(new Vector2(currentLayer, angleArc1), amountNeighbours);
                }
            }
        }
    }
}