using Assets.Code;
using UnityEngine;

public class Player : MonoBehaviour, ILoad
{
	public Vector3? NextLocation;
	public bool Stuned;

	public void Load()
	{
		NextLocation = null;
		UnStun();
	}

	public void UnStun()
	{
		GetComponent<SpriteRenderer>().color = Color.white;
		var animator = GetComponent<Animator>();

		if(animator != null)
		{
			animator.enabled = true;
		}
		Stuned = false;
	}

	internal void Stun(Color stunColor)
	{
		var dimColorValue = 0.5f;
		GetComponent<SpriteRenderer>().color = new Color(stunColor.r * dimColorValue,
														stunColor.g * dimColorValue,
														stunColor.b * dimColorValue);
		var animator = GetComponent<Animator>();

		if(animator != null)
		{
			animator.enabled = false;
		}

		Stuned = true;
	}

	private void Update()
	{
		if(NextLocation == null)
		{
			return;
		}

		transform.position = Vector2.MoveTowards(transform.position,
				NextLocation.Value,
				GlobalSettings.MOVEMENT_SPEED * Time.deltaTime);
	}
}