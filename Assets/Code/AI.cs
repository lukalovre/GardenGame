﻿using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour
{
	public static bool DoTurn;
	private Collider2D collider;
	private GameObject m_path;
	private Vector3 NextLocation;
	private List<Vector3> Path = new List<Vector3>();
	private Vector3 StartLocation;
	public bool DoneTurn { get; private set; }

	private Vector3 GetRandomDirection()
	{
		var gridObject = GenerateMap.MapMatrix[0, 0];

		var newPosition = gridObject?.GetValidMoveLocations()?.FirstOrDefault();

		if(newPosition == null)
		{
			return transform.position;
		}

		return newPosition.Value;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
	}

	private void SetNextLocationPath()
	{
		m_path.transform.position = Vector2.Lerp(StartLocation, NextLocation, 0.5f);

		if(StartLocation.x != NextLocation.x)
		{
			m_path.transform.rotation = Quaternion.Euler(0, 0, 90);
		}
		else
		{
			m_path.transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}

	// Start is called before the first frame update
	private void Start()
	{
		collider = GetComponent<Collider2D>();
		StartLocation = new Vector3(transform.position.x, transform.position.y);
		NextLocation = GetRandomDirection();

		m_path = Instantiate(GameObject.Find("Path"));
	}

	// Update is called once per frame
	private void Update()
	{
		if(!DoTurn)
		{
			DoneTurn = false;
			SetNextLocationPath();

			return;
		}

		if(Vector3.Distance(transform.position, NextLocation) <= 0f)
		{
			NextLocation = GetRandomDirection();
			StartLocation = new Vector3(transform.position.x, transform.position.y);
			DoneTurn = true;
		}
		else
		{
			transform.position = Vector2.MoveTowards(transform.position,
NextLocation,
GlobalSettings.MOVEMENT_SPEED * Time.deltaTime);
		}
	}
}