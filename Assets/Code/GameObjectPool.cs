using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
	public static class GameObjectPool
	{
		public static Vector3 PoolLocation = new Vector3(-1, -1, -20);
		private static List<GameObject> s_gameObjectPool = new List<GameObject>();

		public static GameObject Create(GameObject prototype)
		{
			s_gameObjectPool.RemoveAll(o => o == null);

			var foundObject = s_gameObjectPool.FirstOrDefault(o => o.CompareTag(prototype.tag));

			if(foundObject != null)
			{
				s_gameObjectPool.Remove(foundObject);

				foundObject.SetActive(true);
				return foundObject;
			}

			var newGameObject = GameObject.Instantiate(prototype);

			return newGameObject;
		}

		public static void Delete(GameObject gameObject)
		{
			gameObject.SetActive(false);
			gameObject.transform.position = PoolLocation;
			s_gameObjectPool.Add(gameObject);
		}
	}
}