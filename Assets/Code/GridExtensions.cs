using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
	public static class GridExtensions
	{
		public static readonly Vector3[] Directions = new Vector3[]
{
		new Vector3(1, 0),
		new Vector3(-1, 0),
		new Vector3(0, 1),
		new Vector3(0, -1),
};

		public static List<Vector3> GetEmptyTiles(this bool[,] grid)
		{
			var emptyTiles = new List<Vector3>();

			var width = grid.GetLength(0);
			var height = grid.GetLength(1);

			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					var isBlockingTile = grid[x, y];

					if(!isBlockingTile)
					{
						emptyTiles.Add(new Vector3(x, y));
					}
				}
			}

			return emptyTiles;
		}

		public static List<Vector3> GetValidNeighbors(this bool[,] grid, Vector3 position)
		{
			var neighbors = new List<Vector3>(Directions.Length);

			foreach(var direction in Directions)
			{
				var neighborPosition = position + direction;

				if(grid.IsInRange(neighborPosition)
					// Grid is not blocked
					&& !grid[(int)neighborPosition.x, (int)neighborPosition.y])
				{
					neighbors.Add(new Vector3(neighborPosition.x, neighborPosition.y));
				}
			}

			return neighbors;
		}

		public static bool IsInRange(this bool[,] grid, Vector3 position)
		{
			var x = position.x;
			var y = position.y;

			bool isRowValid = 0 <= x && x < grid.GetLength(0);
			bool isColumnValid = 0 <= y && y < grid.GetLength(1);

			return isRowValid && isColumnValid;
		}
	}
}