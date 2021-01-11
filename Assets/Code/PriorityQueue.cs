using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
	public class PriorityQueue
	{
		private Comparison<Vector3> heuristicComparison;

		private Queue<Tuple<Vector3, float>> Queue = new Queue<Tuple<Vector3, float>>();

		public PriorityQueue(Comparison<Vector3> heuristicComparison)
		{
			this.heuristicComparison = heuristicComparison;
		}

		public int Count => Queue.Count;

		internal Vector3 Dequeue()
		{
			return Queue.Dequeue().Item1;
		}

		internal void Enqueue(Vector3 start)
		{
			Queue.Enqueue(new Tuple<Vector3, float>(start, 0));
		}
	}
}