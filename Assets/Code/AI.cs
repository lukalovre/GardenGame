using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
		transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 0), 1 * Time.deltaTime);
	}
}