using UnityEngine;

public class Explosion : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		var snail = collision.GetComponent<AI>();

		if(snail != null)
		{
			snail.Die();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		var snail = collision.GetComponent<AI>();

		if(snail != null)
		{
			snail.Die();
		}
	}

	private void Update()
	{
	}
}