using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Pathfinding
{
	public static class UniformCostSearch
	{
		public static List<Vector3?> GetPath(Vector3 start, Vector3? end, GridObject[,] grid)
		{
			var costs = InitializePathCosts(grid);
			costs[start] = 0.0f;

			var visited = new Dictionary<Vector3?, Vector3?>();
			visited.Add(start, null);

			var frontier = new PriorityQueue((a, b) => costs[a].CompareTo(costs[b]));
			frontier.Enqueue(start);

			while(frontier.Count > 0)
			{
				var current = frontier.Dequeue();

				if(current == end)
				{
					break;
				}

				var neighbors = grid[(int)current.x, (int)current.y].GetValidMoveLocations();

				foreach(var tile in neighbors)
				{
					var currentCost = costs[tile];
					var newCost = costs[current] /*+ tile.Weight*/;

					if(newCost < currentCost)
					{
						costs[tile] = newCost;

						if(visited.ContainsKey(tile))
						{
							visited[tile] = current;
						}
					}

					if(!visited.ContainsKey(tile))
					{
						frontier.Enqueue(tile);
						visited.Add(tile, current);
					}
				}
			}

			return GetPathTo(end, visited);
		}

		private static List<Vector3?> GetPathTo(Vector3? end, Dictionary<Vector3?, Vector3?> visited)
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

		private static IDictionary<Vector3, float> InitializePathCosts(GridObject[,] grid)
		{
			var costs = new Dictionary<Vector3, float>();

			foreach(var tile in grid)
			{
				costs.Add(tile.Position, float.PositiveInfinity);
			}

			return costs;
		}
	}
}