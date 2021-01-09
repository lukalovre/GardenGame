﻿using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	public static bool DoTurn;
	private Collider2D collider;
	private Vector3? NextLocation;

	private List<Vector3> Path = new List<Vector3>();
	public bool DoneTurn { get; private set; }

	private Vector3 GetRandomDirection()
	{
		var randomDirection = Random.Range(1, 5);
		var randomDirectionVector = new Vector3(0, 0);

		if(randomDirection == 1)
		{
			randomDirectionVector = new Vector3(-1, 0);
		}

		if(randomDirection == 2)
		{
			randomDirectionVector = new Vector3(0, 1);
		}

		if(randomDirection == 3)
		{
			randomDirectionVector = new Vector3(1, 0);
		}

		if(randomDirection == 4)
		{
			randomDirectionVector = new Vector3(0, -1);
		}

		return transform.position + randomDirectionVector;
	}

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
			DoneTurn = false;
			return;
		}

		if(NextLocation == null)
		{
			NextLocation = GetRandomDirection();
		}

		if(transform.position == NextLocation)
		{
			NextLocation = null;
			DoneTurn = true;
		}
		else
		{
			transform.position = Vector2.MoveTowards(transform.position,
				NextLocation.Value,
				1 * Time.deltaTime);
		}
	}
}