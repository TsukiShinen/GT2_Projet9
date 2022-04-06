using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GameParameters parameters;
    [SerializeField] private LayerMask terrainMasks;

    public Vector2Int gridSize;
    public float cellSize = 1f;

    private Cell[,] _grid;
    private float CellDiameter => cellSize;
    private float CellRadius => cellSize / 2f;
    
    
    private FlowField _currentFlowField;

    private void InitializeFlowField()
    {
        _currentFlowField = new FlowField(_grid, gridSize, cellSize, parameters, terrainMasks);
    }
    
    private void CreateGrid()
    {
        _grid = new Cell[gridSize.x, gridSize.y];

        for (var x = 0; x < gridSize.x; x++)
        {
            for (var y = 0; y < gridSize.y; y++)
            {
                var worldPos = new Vector2(CellDiameter * x + CellRadius, CellDiameter * y + CellRadius);
                _grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
            }
        }
    }

    private void CreateCostField()
    {

        foreach (var cell in _grid)
        {
            var origin = new Vector3(cell.WorldPosition.x, cell.WorldPosition.y, -10);
            var size = new Vector2(CellDiameter -0.1f, CellDiameter-0.1f);
            var hit = Physics2D.BoxCast(origin, size, 0f, Vector3.forward, 20f, terrainMasks);
            
            if (hit.collider == null) { continue; }
            if (hit.collider.gameObject.layer == parameters.LayerNavigationObstacleAsLayer)
            {
                cell.IncreaseCost(255);
            }
            else if(hit.collider.gameObject.layer == parameters.GroundNormalAsLayer)
            {
                cell.IncreaseCost(1);
            } 
            else if (hit.collider.gameObject.layer == parameters.LayerGroundSlowAsLayer)
            {
                cell.IncreaseCost(2);
            }
        }
    }

    private void Start()
    {
        CreateGrid();
        CreateCostField();
        InitializeFlowField();
    }

    public Cell[,] GenerateFlowField(Vector2 position)
    {
        var destinationCell = GetCellFromWorldPosition(_grid, cellSize, position);
        _currentFlowField.CreateIntegrationField(destinationCell);
        _currentFlowField.CreateFlowField();
        return _currentFlowField.Grid;
    }
    
    public static List<Cell> GetNeighborCells(Cell[,] grid, Vector2Int nodeIndex, List<GridDirection> directions)
    {
        var neighbors = new List<Cell>();

        foreach (var direction in directions)
        {
            var newNeighbor = GetCellAtRelativePosition(grid, nodeIndex, direction);
            if (newNeighbor == null) { continue; }
            neighbors.Add(newNeighbor);
        }
        
        return neighbors;
    }

    private static Cell GetCellAtRelativePosition(Cell[,] grid, Vector2Int originPosition, Vector2Int relativePosition)
    {
        var finalPosition = originPosition + relativePosition;

        if (finalPosition.x < 0 || finalPosition.x >= grid.GetLength(0) || finalPosition.y < 0 || finalPosition.y >= grid.GetLength(1)) { return null; }

        return grid[finalPosition.x, finalPosition.y];
    }
    
    public static Cell GetCellFromWorldPosition(Cell[,] grid, float cellSize, Vector2 worldPosition)
    {
        var percentX = worldPosition.x / (grid.GetLength(0) * cellSize);
        var percentY = worldPosition.y / (grid.GetLength(1) * cellSize);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        var x = Mathf.Clamp(Mathf.FloorToInt((grid.GetLength(0)) * percentX), 0, grid.GetLength(0) - 1); 
        var y = Mathf.Clamp(Mathf.FloorToInt((grid.GetLength(1)) * percentY), 0, grid.GetLength(1) - 1);
        return grid[x, y];
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        /*if (CurrentFlowField != null)
        {
            DrawGrid(Color.green);

            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;

            foreach (var cell in CurrentFlowField.Grid)
            {
                Handles.Label(cell.WorldPosition, cell.Cost.ToString(), style);
                //var to = cell.WorldPosition + new Vector2(cell.BestDirection.Vector.x, cell.BestDirection.Vector.y) / 2;
            }
        }
        else
        {
            //DrawGrid(Color.yellow);
        }*/
    }

    private void DrawGrid(Color color)
    {
        Gizmos.color = color;
        for (var x = 0; x < gridSize.x; x++)
        {
            for (var y = 0; y < gridSize.y; y++)
            {
                var center = new Vector2(cellSize * x + cellSize / 2, cellSize * y + cellSize / 2);
                var size = Vector2.one * cellSize;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
    #endregion
}
