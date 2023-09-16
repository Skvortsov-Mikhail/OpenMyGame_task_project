using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            List<Cell> matrix = CreateMatrix(grid);

            UpdateMatrixByCellSteps(matrix, unit, from, grid.Size);

            List<Vector2Int> path = GetPathByMatrix(matrix, to);

            if (path == null)
            {
                Debug.Log($"Нет доступных ходов для перемещения фигуры {unit} в клетку {to}");
            }

            return path;
        }

        private List<Cell> CreateMatrix(ChessGrid grid)
        {
            List<Cell> matrix = new List<Cell>();

            for (int i = 0; i < grid.Size.x; i++)
            {
                for (int j = 0; j < grid.Size.y; j++)
                {
                    matrix.Add(new Cell(new Vector2Int(i, j)));
                }
            }

            foreach (var piece in grid.Pieces)
            {
                matrix.Find(el => el.coordinates == piece.CellPosition).isAvailable = false;
            }

            return matrix;
        }

        private void UpdateMatrixByCellSteps(List<Cell> matrix, ChessUnitType unit, Vector2Int from, Vector2Int size)
        {
            matrix.Find(el => el.coordinates == from).stepNumber = 0;

            int stepNumber = 1;

            List<Cell> list = new List<Cell> { matrix.Find(el => el.coordinates == from) };

            while (matrix.Find(el => (el.isAvailable && el.stepNumber == -1)) != null)
            {
                List<Cell> neighboursList = new List<Cell>();

                foreach (Cell cell in list)
                {
                    neighboursList.AddRange(GetNeighbours(matrix, unit, matrix.Find(el => el == cell), size));

                    foreach (Cell neighbourCell in neighboursList)
                    {
                        Cell currentCell = matrix.Find(el => el == neighbourCell);

                        if (currentCell.stepNumber != -1) continue;

                        currentCell.stepNumber = stepNumber;
                        currentCell.parent = cell;
                    }
                }

                if (neighboursList.Count == 0)
                {
                    foreach (Cell cell in matrix)
                    {
                        if (cell.parent == null)
                        {
                            cell.isAvailable = false;
                        }
                    }
                }

                list = new List<Cell>(neighboursList);

                stepNumber++;
            }
        }

        private List<Vector2Int> GetPathByMatrix(List<Cell> matrix, Vector2Int to)
        {
            List<Vector2Int> path = new List<Vector2Int>();

            Cell targetCell = matrix.Find(el => el.coordinates == to);

            if (targetCell.isAvailable == false) return null;

            path.Insert(0, targetCell.coordinates);

            Cell parentCell = targetCell.parent;

            while (parentCell.parent != null)
            {
                path.Insert(0, parentCell.coordinates);

                parentCell = parentCell.parent;
            }

            return path;
        }

        private List<Cell> GetNeighbours(List<Cell> matrix, ChessUnitType unit, Cell cell, Vector2Int size)
        {
            List<Cell> list = new List<Cell>();

            switch (unit)
            {
                case ChessUnitType.Pon:

                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x, cell.coordinates.y + 1), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x, cell.coordinates.y - 1), list, size);

                    break;

                case ChessUnitType.King:
                    
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x, cell.coordinates.y + 1), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x, cell.coordinates.y - 1), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x + 1, cell.coordinates.y), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x - 1, cell.coordinates.y), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x + 1, cell.coordinates.y + 1), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x - 1, cell.coordinates.y + 1), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x + 1, cell.coordinates.y - 1), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x - 1, cell.coordinates.y - 1), list, size);

                    break;

                case ChessUnitType.Queen:

                    TryAddVerticalAndHorizontalAxis(matrix, list, cell, size);
                    TryAddDiagonals(matrix, list, cell, size);

                    break;

                case ChessUnitType.Rook:

                    TryAddVerticalAndHorizontalAxis(matrix, list, cell, size);

                    break;

                case ChessUnitType.Knight:

                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x + 1, cell.coordinates.y + 2), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x + 2, cell.coordinates.y + 1), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x + 1, cell.coordinates.y - 2), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x + 2, cell.coordinates.y - 1), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x - 1, cell.coordinates.y + 2), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x - 2, cell.coordinates.y + 1), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x - 1, cell.coordinates.y - 2), list, size);
                    TryAddSingleCell(matrix, new Vector2Int(cell.coordinates.x - 2, cell.coordinates.y - 1), list, size);

                    break;

                case ChessUnitType.Bishop:

                    TryAddDiagonals(matrix, list, cell, size);

                    break;
            }

            return list;
        }

        private void TryAddVerticalAndHorizontalAxis(List<Cell> matrix, List<Cell> list, Cell cell, Vector2Int size)
        {
            // up
            for (int j = cell.coordinates.y + 1; j < size.y; j++)
            {
                if (TryAddCellInRow(matrix, new Vector2Int(cell.coordinates.x, j), list) == false) break;
            }
            // down
            for (int j = cell.coordinates.y - 1; j >= 0; j--)
            {
                if (TryAddCellInRow(matrix, new Vector2Int(cell.coordinates.x, j), list) == false) break;
            }
            // right
            for (int i = cell.coordinates.x + 1; i < size.x; i++)
            {
                if (TryAddCellInRow(matrix, new Vector2Int(i, cell.coordinates.y), list) == false) break;
            }
            // left
            for (int i = cell.coordinates.x - 1; i >= 0; i--)
            {
                if (TryAddCellInRow(matrix, new Vector2Int(i, cell.coordinates.y), list) == false) break;
            }
        }

        private void TryAddDiagonals(List<Cell> matrix, List<Cell> list, Cell cell, Vector2Int size)
        {
            //upright
            for (int k = 1; k < size.x - cell.coordinates.x && k < size.y - cell.coordinates.y; k++)
            {
                if (TryAddCellInRow(matrix, new Vector2Int(cell.coordinates.x + k, cell.coordinates.y + k), list) == false) break;
            }
            //upleft
            for (int k = 1; k <= cell.coordinates.x && k < size.y - cell.coordinates.y; k++)
            {
                if (TryAddCellInRow(matrix, new Vector2Int(cell.coordinates.x - k, cell.coordinates.y + k), list) == false) break;
            }
            //downright
            for (int k = 1; k < size.x - cell.coordinates.x && k <= cell.coordinates.y; k++)
            {
                if (TryAddCellInRow(matrix, new Vector2Int(cell.coordinates.x + k, cell.coordinates.y - k), list) == false) break;
            }
            //downleft
            for (int k = 1; k <= cell.coordinates.x && k <= cell.coordinates.y; k++)
            {
                if (TryAddCellInRow(matrix, new Vector2Int(cell.coordinates.x - k, cell.coordinates.y - k), list) == false) break;
            }
        }

        private void TryAddSingleCell(List<Cell> matrix, Vector2Int coordinates, List<Cell> list, Vector2Int size)
        {
            if (coordinates.x < 0 || coordinates.x > size.x) return;
            if (coordinates.y < 0 || coordinates.y > size.y) return;

            Cell newCell = matrix.Find(el => el.coordinates == coordinates && el.isAvailable && el.stepNumber == -1);

            if (newCell != null)
            {
                list.Add(newCell);
            }
        }

        private bool TryAddCellInRow(List<Cell> matrix, Vector2Int coordinates, List<Cell> list)
        {
            if (matrix.Find(el => el.coordinates == coordinates).isAvailable == false) return false;

            Cell newCell = matrix.Find(el => el.coordinates == coordinates && el.stepNumber == -1);

            if (newCell != null)
            {
                list.Add(newCell);
            }

            return true;
        }
    }
}