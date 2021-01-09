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

		var newPosition = transform.position + randomDirectionVector;

		var legalO = GenerateMap.GridObjectList.FirstOrDefault(o =>
		  o.Position == new Vector2Int((int)newPosition.x, (int)newPosition.y)
		  && o.ObjectType == GridObject.Type.Empty);

		return newPosition;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
	}

	// Start is called before the first frame update
	private void Start()
	{
		collider = GetComponent<Collider2D>();
		StartLocation = new Vector3(transform.position.x, transform.position.y);
		NextLocation = GetRandomDirection();

		m_path = Instantiate(GameObject.Find("Path"));
		m_path.transform.position = transform.position;
	}

	// Update is called once per frame
	private void Update()
	{
		if(!DoTurn)
		{
			DoneTurn = false;
			m_path.transform.position = Vector2.Lerp(StartLocation, NextLocation, 0.5f);

			if(StartLocation.x != NextLocation.x)
			{
				m_path.transform.rotation = Quaternion.Euler(0, 0, 90);
			}
			else
			{
				m_path.transform.rotation = Quaternion.Euler(0, 0, 0);
			}

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