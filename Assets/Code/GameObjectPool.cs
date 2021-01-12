using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
	public static class GameObjectPool
	{
		private static List<GameObject> s_gameObjectPool = new List<GameObject>();
		private static Vector3 s_poolLocation = new Vector3(-1, -1);

		public static GameObject Create(GameObject prototype)
		{
			var foundObject = s_gameObjectPool.FirstOrDefault(o => o.CompareTag(prototype.tag));

			if(foundObject != null)
			{
				s_gameObjectPool.Remove(foundObject);
				return foundObject;
			}

			var newGameObject = GameObject.Instantiate(prototype);

			return newGameObject;
		}

		public static void Delete(GameObject gameObject)
		{
			gameObject.transform.position = s_poolLocation;
			s_gameObjectPool.Add(gameObject);
		}
	}
}