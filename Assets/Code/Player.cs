using Assets.Code;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Vector3? NextLocation;

	private void Start()
	{
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