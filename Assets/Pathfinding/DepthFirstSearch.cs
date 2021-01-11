using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Pathfinding
{
	public static class DepthFirstSearch
	{
		public static List<Vector3?> GetPath(Vector3 start, Vector3? end, bool[,] grid)
		{
			var visited = new Dictionary<Vector3?, Vector3?>();
			visited.Add(start, null);

			var frontier = new Stack<Vector3>();
			frontier.Push(start);

			while(frontier.Count > 0)
			{
				var current = frontier.Pop();

				if(current == end)
				{
					break;
				}

				var neighbors = Helper.Shuffle(grid.GetNeighbors((int)current.x, (int)current.y));

				foreach(var neighbour in neighbors)
				{
					if(!visited.ContainsKey(neighbour))
					{
						frontier.Push(neighbour);
						visited.Add(neighbour, current);
					}
				}
			}

			return Helper.GetPathTo(end, visited);
		}
	}
}