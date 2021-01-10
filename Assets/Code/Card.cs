﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
	public GameObject Player;
	public CardType Type;
	private List<GameObject> aiList;
	private Collider2D collider;

	public enum CardType
	{
		Up,
		Down,
		Left,
		Right
	}

	private void Move(Vector3 vector3)
	{
		var x = (int)Player.transform.position.x;
		var y = (int)Player.transform.position.y;

		var gridObject = GenerateMap.MapMatrix[x, y];

		var validNextPositions = gridObject?.GetValidMoveLocations();

		if(validNextPositions.Contains(Player.transform.position + vector3))
		{
			Player.GetComponent<Player>().NextLocation = Player.transform.position + vector3;
		}
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
		aiList = GameObject.FindGameObjectsWithTag("AI").ToList();

		if(aiList.All(ai => ai.GetComponent<AI>().DoneTurn))
		{
			AI.DoTurn = false;
		}

		if(Input.touchCount == 0)
		{
			return;
		}

		var touch = Input.GetTouch(0);
		var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
		var touchedCollider = Physics2D.OverlapPoint(touchPosition);

		if(collider == touchedCollider && touch.phase == TouchPhase.Began && !AI.DoTurn)
		{
			AI.DoTurn = true;

			switch(Type)
			{
				case CardType.Up:

					Move(new Vector3(0, 1));
					break;

				case CardType.Down:
					Move(new Vector3(0, -1));
					break;

				case CardType.Left:
					Move(new Vector3(-1, 0));
					break;

				case CardType.Right:
					Move(new Vector3(1, 0));
					break;

				default:
					break;
			}
		}
	}
}