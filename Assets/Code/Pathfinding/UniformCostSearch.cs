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

				var neighbors = grid.GetValidNeighbors(current);

				foreach(var neighbour in neighbors)
				{
					var currentCost = costs[neighbour];
					var newCost = costs[current] /*+ neighbour.Weight*/;

					if(newCost < currentCost)
					{
						costs[neighbour] = newCost;

						if(visited.ContainsKey(neighbour))
						{
							visited[neighbour] = current;
						}
					}

					if(!visited.ContainsKey(neighbour))
					{
						frontier.Enqueue(neighbour);
						visited.Add(neighbour, current);
					}
				}
			}

			return Helper.GetPathTo(end, visited);
		}
	}
}