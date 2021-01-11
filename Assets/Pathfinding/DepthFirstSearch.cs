using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Pathfinding
{
	public static class DepthFirstSearch
	{
		public static List<Vector3?> GetPath(Vector3 start, Vector3? end, GridObject[,] grid)
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

				var neighbors = Helper.Shuffle(grid[(int)current.x, (int)current.y].GetValidMoveLocations());

				foreach(var neighbour in neighbors)
				{
					if(!visited.ContainsKey(neighbour))
					{
						frontier.Push(neighbour);
						visited.Add(neighbour, current);
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
	}
}