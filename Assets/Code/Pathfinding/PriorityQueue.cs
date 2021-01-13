using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Pathfinding
{
	public class PriorityQueue
	{
		private readonly List<Tuple<Vector3, float>> List = new List<Tuple<Vector3, float>>();

		public int Count => List.Count;

		internal Vector3 Dequeue()
		{
			var maxValue = List.Max(o => o.Item2);
			var foundElement = List.FirstOrDefault(o => o.Item2 == maxValue);

			List.Remove(foundElement);

			return foundElement.Item1;
		}

		internal void Enqueue(Vector3 position, float value)
		{
			List.Add(new Tuple<Vector3, float>(position, value));
		}
	}
}