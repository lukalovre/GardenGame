﻿using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Pathfinding
{
	public static class AStarSearch
	{
		public static List<Vector3?> GetPath(Vector3? start, Vector3? end, bool[,] grid)
		{
			Func<Vector3, Vector3, float> heuristic = CalculateManhattanDistance;

			var costs = Helper.InitializePathCosts(grid);
			costs[start.Value] = 0.0f;

			var visited = new Dictionary<Vector3?, Vector3?>();
			visited.Add(start, null);

			var frontier = new PriorityQueue();

			frontier.Enqueue(start.Value, costs[start.Value] + heuristic(start.Value, end.Value));

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
						frontier.Enqueue(neighbour, costs[neighbour] + heuristic(neighbour, end.Value));

						visited.Add(neighbour, current);
					}
				}
			}

			return Helper.GetPathTo(end, visited);
		}

		private static float CalculateManhattanDistance(Vector3 a, Vector3 b)
		{
			return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
		}
	}
}