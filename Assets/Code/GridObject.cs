using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Code
{
	public class GridObject
	{
		public Type ObjectType;
		public Vector2Int Position;

		public enum Type
		{
			Empty,
			Rock
		}

		public void Create()
		{
			if(ObjectType == Type.Rock)
			{
				var someGameObject = GameObject.Find("Rock");
				var go = Object.Instantiate(someGameObject);
				go.transform.position = (Vector2)Position;
			}
		}
	}
}