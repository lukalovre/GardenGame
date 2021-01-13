using Assets.Code;
using UnityEngine;

public class Slimeball : MonoBehaviour
{
	private int m_parentID;

	public void Fire(int parentID, Vector3 startPosition, Vector3 direction)
	{
		m_parentID = parentID;
		transform.position = startPosition;
		GetComponent<Rigidbody2D>().velocity = direction * GlobalSettings.SLIMEBALL_SPEED;
	}

	private void OnTriggerEnter2D(Collider2D hitInfo)
	{
		var player = hitInfo.GetComponent<Player>();

		if(player != null)
		{
			player.Stun(GetComponent<SpriteRenderer>().color);
		}

		var snack = hitInfo.GetComponent<Snack>();

		if(snack != null)
		{
			snack.Bite();
		}

		var snail = hitInfo.GetComponent<AI>();

		if(snail != null && snail.GetInstanceID() != m_parentID)
		{
			snail.Die();
		}

		if(snail != null && snail.GetInstanceID() == m_parentID)
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