using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
	public class GridObject
	{
		public GameObject GameObject;
		public Type ObjectType;
		public Vector2Int Position;

		public enum Type
		{
			Empty,
			Rock,
			Ant,
			PC
		}

		public void Create()
		{
			var foundObject = FindObject();

			if(foundObject == null)
			{
				return;
			}

			GameObject = Object.Instantiate(foundObject);
			GameObject.transform.position = (Vector2)Position;
		}

		public List<Vector3> GetValidMoveLocations()
		{
			var up = GenerateMap.GridObjectList.FirstOrDefault(o => o.Position == Position + new Vector2Int(0, 1));
			var down = GenerateMap.GridObjectList.FirstOrDefault(o => o.Position == Position + new Vector2Int(0, -1));
			var left = GenerateMap.GridObjectList.FirstOrDefault(o => o.Position == Position + new Vector2Int(-1, 0));
			var right = GenerateMap.GridObjectList.FirstOrDefault(o => o.Position == Position + new Vector2Int(1, 0));

			var list = new List<GridObject>
			{
				up,
				down,
				left,
				right
			};

			list.RemoveAll(o => o == null);
			list.RemoveAll(o => o.ObjectType != Type.Empty);

			return list.Select(o => new Vector3(o.Position.x, o.Position.y, 0)).ToList();
		}

		private GameObject FindObject()
		{
			if(ObjectType == Type.Empty)
			{
				return null;
			}

			return GameObject.Find(ObjectType.ToString());
		}
	}
}