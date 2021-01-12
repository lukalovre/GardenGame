using Assets.Code;
using UnityEngine;

public class Slimeball : MonoBehaviour
{
	public void Fire(Vector3 startPosition, Vector3 direction)
	{
		transform.position = startPosition;
		GetComponent<Rigidbody2D>().velocity = direction * GlobalSettings.SLIMEBALL_SPEED;
	}

	private void OnTriggerEnter2D(Collider2D hitInfo)
	{
		var player = hitInfo.GetComponent<Player>();

		if(player != null)
		{
			player.Stun();
		}

		var strawberry = hitInfo.GetComponent<Strawberry>();

		if(strawberry != null)
		{
			strawberry.Bite();
		}

		GameObjectPool.Delete(gameObject);
	}

	private void Update()
	{
		if(!GenerateMap.Grid.IsInRange(transform.position))
		{
			GameObjectPool.Delete(gameObject);
		}
	}
}