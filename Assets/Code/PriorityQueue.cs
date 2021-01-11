using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code
{
	public class PriorityQueue<T>
	{
		private Comparison<Vector3> heuristicComparison;

		public PriorityQueue(Comparison<Vector3> heuristicComparison)
		{
			this.heuristicComparison = heuristicComparison;
		}

		public int Count { get; set; }

		internal Vector3 Dequeue()
		{
			throw new NotImplementedException();
		}

		internal void Enqueue(Vector3? start)
		{
			throw new NotImplementedException();
		}
	}
}