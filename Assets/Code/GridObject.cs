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

			GameObject = GameObject.Instantiate(foundObject);
			GameObject.transform.position = (Vector2)Position;
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