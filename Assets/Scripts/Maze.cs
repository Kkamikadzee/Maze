using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Maze
{
    public MazeCell[] Cells;
    public MazeCell FinishCell;

    public byte[,] AdjacencMatrix;
    public int AmountCells => Cells.Length;

    private int indexStartCell = -1;
    public int IndexStartCell
    {
        get
        {
            if (indexStartCell != -1)
            {
                return indexStartCell;
            }
            else
            {
                foreach ((MazeCell cell, int index) in Cells.Select((value, i) => (value, i)))
                {
                    if (cell.position == PositionStart)
                    {
                        indexStartCell = index;
                        return index;
                    }
                }
            }
            return -1;
        }
    }
    public Vector2 PositionStart;
    public Vector2 PositionFinish;
    public MazeCell StartCell
    {
        get
        {
            foreach((MazeCell cell, int index) in Cells.Select((value, i) => (value, i)))
            {
                if(cell.position == PositionStart)
                {
                    indexStartCell = index;
                    return cell;
                }
            }
            return null;
        }
    }

    public Maze() { }

    public void InitializeOrthogonalMaze(int width, int height, Vector2Int positionStart)
    {
        this.PositionStart = positionStart;
        Cells =  new MazeCell[width * height];
        AdjacencMatrix = new byte[AmountCells, AmountCells];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                #region Заполнение матрицы смежности
                if (x - 1 >= 0)
                {
                    AdjacencMatrix[(x) + width * (y), (x - 1) + width * (y)] = 1;
                    AdjacencMatrix[(x - 1) + width * (y), (x) + width * (y)] = 1;
                }
                if (x + 1 < width)
                {
                    AdjacencMatrix[(x) + width * (y), (x + 1) + width * (y)] = 1;
                    AdjacencMatrix[(x + 1) + width * (y), (x) + width * (y)] = 1;
                }
                if (y - 1 >= 0)
                {
                    AdjacencMatrix[(x) + width * (y), (x) + width * (y - 1)] = 1;
                    AdjacencMatrix[(x) + width * (y - 1), (x) + width * (y)] = 1;
                }
                if (y + 1 < height)
                {
                    AdjacencMatrix[(x) + width * (y), (x) + width * (y + 1)] = 1;
                    AdjacencMatrix[(x) + width * (y + 1), (x) + width * (y)] = 1;
                }
                #endregion
                Cells[x + width * y] = new MazeCell(new Vector2(x, y), 4);
            }
        }
    }
    /// <summary>
    /// Инициализирует тета-лабиринт
    /// </summary>
    /// <param name="outerDiameter">Наружный диаметр</param>
    /// <param name="InnerDiameter">Диаметр центральной ячейки</param>
    /// <param name="firstLayer">Количество ячеек на первом слое</param>
    /// <param name="bias">Приоритет круговых коридоров</param>
    public void InitializeThetaMaze(int outerDiameter, int firstLayer, bool bias)
    {
        byte weightSideWalls; //Веса боковых стен
        byte weightCircularWalls; //Веса стен на сосдних слоях

        if(bias)
        {
            weightSideWalls = 1;
            weightCircularWalls = 2;
        }
        else
        {
            weightSideWalls = 1;
            weightCircularWalls = 1;
        }

        this.PositionStart = Vector2.zero;
        int amountCells = 1 + firstLayer * (outerDiameter - 1) + (outerDiameter - 2) * (outerDiameter - 1) * 2; //Кол-во ячеек, если увеличивать кол-во на 1 в каждой четверти на следующем слое
        Cells = new MazeCell[amountCells];
        AdjacencMatrix = new byte[amountCells, amountCells];
        //Центральная ячейка
        for (int i = 1; i <= firstLayer; i++)
        {
            AdjacencMatrix[0, i] = weightCircularWalls;
            AdjacencMatrix[i, 0] = weightCircularWalls;
        }
        Cells[0] = new MazeCell(Vector2.zero, firstLayer);
        //Остальные
        int amountCellsInPreviousLayers;
        int amountCellsInCurrentLayer;
        int amountCellsInNextLayer;
        for(int currentLayer = 0; currentLayer < outerDiameter - 1; currentLayer++)
        {
            amountCellsInPreviousLayers = 1 + firstLayer * (currentLayer) + (currentLayer - 1) * (currentLayer) * 2;
            amountCellsInCurrentLayer = firstLayer + (4 * currentLayer);
            amountCellsInNextLayer = firstLayer + (4 * (currentLayer + 1));
            for(int currentCellInLayer = 0; currentCellInLayer < amountCellsInCurrentLayer; currentCellInLayer++)
            {
                int previousCellInLayer = currentCellInLayer > 0 ? currentCellInLayer - 1 : amountCellsInCurrentLayer - 1;
                int nextCellInLayer = (currentCellInLayer + 1) % amountCellsInCurrentLayer;
                //Соседние ячейки на одном слое
                AdjacencMatrix[amountCellsInPreviousLayers + currentCellInLayer, amountCellsInPreviousLayers + previousCellInLayer] = weightSideWalls;
                AdjacencMatrix[amountCellsInPreviousLayers + previousCellInLayer, amountCellsInPreviousLayers + currentCellInLayer] = weightSideWalls;
                AdjacencMatrix[amountCellsInPreviousLayers + currentCellInLayer, amountCellsInPreviousLayers + nextCellInLayer] = weightSideWalls;
                AdjacencMatrix[amountCellsInPreviousLayers + nextCellInLayer, amountCellsInPreviousLayers + currentCellInLayer] = weightSideWalls;

                float angleArc1 = (2 * Mathf.PI / amountCellsInCurrentLayer) * currentCellInLayer;

                float angleArc2 = (2 * Mathf.PI / amountCellsInCurrentLayer) * (currentCellInLayer + 1);
                float angleArcInNextLayer = 2 * Mathf.PI / amountCellsInNextLayer;

                int neighbourArc =
                    Mathf.RoundToInt(
                        angleArc1 / angleArcInNextLayer);
                int amountNeighbours = 0;
                float angleArcNeighbour = angleArcInNextLayer * neighbourArc;
                while (angleArcNeighbour < angleArc2)
                {
                    if (currentLayer < outerDiameter - 2) // -2, ибо -1, чтобы попасть на границу и ещё -1, ибо пропускаем центральную ячейку
                    {
                        AdjacencMatrix[amountCellsInPreviousLayers + currentCellInLayer,
                                amountCellsInPreviousLayers + amountCellsInCurrentLayer + neighbourArc + amountNeighbours] =
                            weightCircularWalls;
                        AdjacencMatrix[amountCellsInPreviousLayers + amountCellsInCurrentLayer + neighbourArc + amountNeighbours,
                            amountCellsInPreviousLayers + currentCellInLayer] = weightCircularWalls;
                    }
                    
                    amountNeighbours++;
                    angleArcNeighbour = angleArcInNextLayer * (neighbourArc + amountNeighbours);
                }

                Cells[amountCellsInPreviousLayers + currentCellInLayer] =
                    new MazeCell(new Vector2(currentLayer, angleArc1), amountNeighbours);
            }
        }
    }
}