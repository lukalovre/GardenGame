using Assets.Code;
using UnityEngine;

public class Player : MonoBehaviour, ILoad
{
	public Vector3? NextLocation;
	public bool Stuned;

	public void Load()
	{
		NextLocation = null;
	}

	public void UnStun()
	{
		GetComponent<SpriteRenderer>().color = Color.white;
		Stuned = false;
	}

	internal void Stun()
	{
		GetComponent<SpriteRenderer>().color = Color.black;
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