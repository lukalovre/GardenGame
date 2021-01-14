using Assets.Code;
using UnityEngine;

public class Slimeball : MonoBehaviour
{
	public void Fire(Vector3 startPosition, Vector3 direction)
	{
		transform.position = startPosition;
		GetComponent<Rigidbody2D>().velocity = direction * GlobalSettings.SLIMEBALL_SPEED;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var player = collision.GetComponent<Player>();

		if(player != null)
		{
			player.Stun(GetComponent<SpriteRenderer>().color);
		}

		var snack = collision.GetComponent<Snack>();

		if(snack != null)
		{
			snack.Bite();
		}

		if(collision.GetComponent<AI>() != null)
		{
			return;
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