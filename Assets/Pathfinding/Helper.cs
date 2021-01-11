using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Pathfinding
{
	public static class Helper
	{
		public static List<Vector3?> GetPathTo(Vector3? end, Dictionary<Vector3?, Vector3?> visited)
		{
			var path = new LinkedList<Vector3?>();

			try
			{
				var current = end;
				var previous = visited[current];

				while(previous != null)
				{
					path.AddFirst(current);

					current = previous;
					previous = visited[current];
				}

				path.AddFirst(current);
			}
			catch(Exception ex)
			{
				return path.ToList();
			}

			return path.ToList();
		}

		public static IDictionary<Vector3, float> InitializePathCosts(bool[,] grid)
		{
			var costs = new Dictionary<Vector3, float>();

			var width = grid.GetLength(0);
			var height = grid.GetLength(1);

			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					costs.Add(new Vector3(x, y), float.PositiveInfinity);
				}
			}

			return costs;
		}

		public static List<Vector3> Shuffle(List<Vector3> tiles)
		{
			// Fisher-Yates shuffle algorithm
			var count = tiles.Count;

			for(var index = 0; index < count; index++)
			{
				int randomIndex = index + UnityEngine.Random.Range(0, count - index);
				var temp = tiles[index];
				tiles[index] = tiles[randomIndex];
				tiles[randomIndex] = temp;
			}

			return tiles;
		}
	}
}