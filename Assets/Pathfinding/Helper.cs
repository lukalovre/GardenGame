using Assets.Code;
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

			var current = end;
			var previous = visited[current];

			while(previous != null)
			{
				path.AddFirst(current);

				current = previous;
				previous = visited[current];
			}

			path.AddFirst(current);

			return path.ToList();
		}

		public static IDictionary<Vector3, float> InitializePathCosts(GridObject[,] grid)
		{
			var costs = new Dictionary<Vector3, float>();

			foreach(var tile in grid)
			{
				costs.Add(tile.Position, float.PositiveInfinity);
			}

			return costs;
		}

		public static List<Vector3> Shuffle(List<Vector3> tiles)
		{
			// Fisher-Yates shuffle algorithm
			var count = tiles.Count;

			for(var index = 0; index < count; index++)
			{
				int randomIndex = index + Random.Range(0, count - index);
				var temp = tiles[index];
				tiles[index] = tiles[randomIndex];
				tiles[randomIndex] = temp;
			}

			return tiles;
		}
	}
}