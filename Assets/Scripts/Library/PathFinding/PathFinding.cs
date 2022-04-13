﻿using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
	public abstract class PathFinding
	{
		public Cell[,] Grid { get; set; }
		public Cell StartingCell { get; set; }
		public Cell DestinationCell { get; set; }
		
		public Vector2Int GridSize { get; set; }
		
		public float CellSize { get; set; }

		public Queue<Vector3> Path;

		public abstract Queue<Vector3> GeneratePath(Vector2 startingPosition, Vector2 destinationPosition);

		public Queue<Vector3> GetPath()
		{
			return Path;
		}

		public void Initialize(Cell[,] baseGrid, float cellSize)
		{
			Grid = baseGrid;
			CellSize = cellSize;
			GridSize = new Vector2Int(Grid.GetLength(0), Grid.GetLength(1));
		}

		#region GetCells
		
		protected List<Cell> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
		{
			var neighbors = new List<Cell>();

			foreach (var direction in directions)
			{
				var newNeighbor = GetCellAtRelativePosition(nodeIndex, direction);
				if (newNeighbor == null) { continue; }
				neighbors.Add(newNeighbor);
			}
        
			return neighbors;
		}

		protected Cell GetCellAtRelativePosition(Vector2Int originPosition, Vector2Int relativePosition)
		{
			var finalPosition = originPosition + relativePosition;

			if (finalPosition.x < 0 || finalPosition.x >= Grid.GetLength(0) || finalPosition.y < 0 || finalPosition.y >= Grid.GetLength(1)) { return null; }

			return Grid[finalPosition.x, finalPosition.y];
		}
    
		protected Cell GetCellFromWorldPosition(Vector2 worldPosition)
		{
			var percentX = worldPosition.x / (GridSize.x * CellSize);
			var percentY = worldPosition.y / (GridSize.y * CellSize);

			percentX = Mathf.Clamp01(percentX);
			percentY = Mathf.Clamp01(percentY);

			var x = Mathf.Clamp(Mathf.FloorToInt((GridSize.x) * percentX), 0, GridSize.x - 1); 
			var y = Mathf.Clamp(Mathf.FloorToInt((GridSize.y) * percentY), 0, GridSize.y - 1);
			return Grid[x, y];
		}

		#endregion
	}
}