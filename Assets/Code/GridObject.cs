using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
	public class GridObject
	{
		public GameObject GameObject;
		public Type ObjectType;
		public Vector3 Position;

		public enum Type
		{
			Empty,
			Rock,
			Player,
			Snail
		}

		private int X => (int)Position.x;
		private int Y => (int)Position.y;

		public void Create()
		{
			var foundObject = FindObject();

			if(foundObject == null)
			{
				return;
			}

			GameObject = GameObject.Instantiate(foundObject);
			GameObject.transform.position = Position;
		}

		public List<Vector3> GetValidMoveLocations()
		{
			var up = GetMatrixValue(X, Y + 1);
			var down = GetMatrixValue(X, Y - 1);
			var left = GetMatrixValue(X - 1, Y);
			var right = GetMatrixValue(X + 1, Y);

			var list = new List<GridObject>
			{
				up,
				down,
				left,
				right
			};

			list.RemoveAll(o => o == null);
			list.RemoveAll(o => o.ObjectType == Type.Rock);

			return list.Select(o => o.Position).ToList();
		}

		private GameObject FindObject()
		{
			if(ObjectType == Type.Empty)
			{
				return null;
			}

			return GameObject.Find(ObjectType.ToString());
		}

		private GridObject GetMatrixValue(int x, int y)
		{
			if(x < 0
				|| y < 0
				|| x >= GenerateMap.MapMatrix.GetLength(0)
				|| y >= GenerateMap.MapMatrix.GetLength(1))
			{
				return null;
			}

			return GenerateMap.MapMatrix[x, y];
		}
	}
}