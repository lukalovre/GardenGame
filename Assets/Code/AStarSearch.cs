using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
	public static class AStarSearch
	{
		public static List<Vector3?> GetPath(Vector3? start, Vector3? end, GridObject[,] grid)
		{
			Func<Vector3, Vector3, float> heuristic = CalculateManhattanDistance;

			var costs = InitializePathCosts(grid);
			costs[start.Value] = 0.0f;

			Comparison<Vector3> heuristicComparison = (a, b) =>
			{
				var aPriority = costs[a] + heuristic(a, end.Value);
				var bPriority = costs[b] + heuristic(b, end.Value);

				return aPriority.CompareTo(bPriority);
			};

			var visited = new Dictionary<Vector3?, Vector3?>();
			visited.Add(start, null);

			var frontier = new PriorityQueue<Vector3?>(heuristicComparison);

			frontier.Enqueue(start);

			while(frontier.Count > 0)
			{
				var current = frontier.Dequeue();

				if(current == end)
				{
					break;
				}

				var neighbors = grid[(int)current.x, (int)current.y].GetValidMoveLocations();

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

			return GetPathTo(end, visited);
		}

		private static float CalculateManhattanDistance(Vector3 a, Vector3 b)
		{
			return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
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