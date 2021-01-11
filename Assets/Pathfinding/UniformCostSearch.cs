using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Pathfinding
{
	public static class UniformCostSearch
	{
		public static List<Vector3?> GetPath(Vector3 start, Vector3? end, bool[,] grid)
		{
			var costs = Helper.InitializePathCosts(grid);
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

				var neighbors = grid.GetValidNeighbors((int)current.x, (int)current.y);

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

			return Helper.GetPathTo(end, visited);
		}
	}
}