using Assets.Code;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Vector3 NextLocation;

	private void Start()
	{
	}

	private void Update()
	{
		transform.position = Vector2.MoveTowards(transform.position,
				NextLocation,
				GlobalSettings.MOVEMENT_SPEED * Time.deltaTime);
	}
}