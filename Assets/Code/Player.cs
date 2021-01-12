using Assets.Code;
using UnityEngine;

public class Player : MonoBehaviour, ILoad
{
	public Vector3? NextLocation;

	public void Load()
	{
		NextLocation = null;
	}

	internal void Hit()
	{
		transform.position = new Vector3(0, 0);
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