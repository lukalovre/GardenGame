using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	public static bool DoTurn;
	private Collider2D collider;
	private Vector2Int NextLocation;
	private List<Vector2Int> Path = new List<Vector2Int>();

	private void OnTriggerEnter2D(Collider2D collision)
	{
	}

	// Start is called before the first frame update
	private void Start()
	{
		collider = GetComponent<Collider2D>();
	}

	// Update is called once per frame
	private void Update()
	{
		if(!DoTurn)
		{
			return;
		}

		transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 0), 1 * Time.deltaTime);
	}
}