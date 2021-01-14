﻿using Assets.Code;
using System;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
	public GameObject Player;
	public CardType Type;
	private const int CARDS_PER_TURN = 2;
	private bool m_clicked;
	private Collider2D m_collider;
	private Vector3 m_startPosition;

	private bool Used;

	public enum CardType
	{
		Up,
		Down,
		Left,
		Right,
		FireNeighbours,
		FireWidth,
		FireHeight
	}

	private static bool AllAIHaveDoneTheirTurn()
	{
		return GameObject.FindGameObjectsWithTag("AI").All(ai => ai.GetComponent<AI>().DoneTurn);
	}

	private void DoCardEffect()
	{
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

			case CardType.FireNeighbours:
				FireNeighbours();
				break;

			case CardType.FireWidth:
				FireWidth();
				break;

			case CardType.FireHeight:
				FireHeight();
				break;

			default:
				break;
		}
	}

	private void FireHeight()
	{
	}

	private void FireNeighbours()
	{
	}

	private void FireWidth()
	{
	}

	private void Move(Vector3 vector3)
	{
		Player.GetComponent<Player>().NextLocation = Player.transform.position + vector3;
	}

	private void OnMouseDown()
	{
		m_clicked = true;
	}

	private void SetCardPositions()
	{
		var vector3 = new Vector3();

		switch(Type)
		{
			case CardType.Up:

				vector3 = new Vector3(0, 1);
				break;

			case CardType.Down:
				vector3 = new Vector3(0, -1);
				break;

			case CardType.Left:
				vector3 = new Vector3(-1, 0);
				break;

			case CardType.Right:
				vector3 = new Vector3(1, 0);
				break;

			default:
				break;
		}

		var validNextPositions = GenerateMap.Grid.GetValidNeighbors(Player.transform.position);

		if(validNextPositions.Contains(Player.transform.position + vector3))
		{
			transform.position = m_startPosition;
		}
		else
		{
			transform.position = m_startPosition + new Vector3(0, -5);
		}
	}

	private void SetStunStatus()
	{
		if(Player.GetComponent<Player>().Stuned)
		{
			GetComponent<SpriteRenderer>().color = Player.GetComponent<SpriteRenderer>().color;
		}
		else
		{
			if(!Used)
			{
				GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
	}

	private void SetUsedStatus(bool used)
	{
		Used = used;
		GetComponent<SpriteRenderer>().color = used ? Color.black : Color.white;
	}

	private void ShuffleCards()
	{
		foreach(var gameObject in GameObject.FindGameObjectsWithTag(tag))
		{
			var card = gameObject.GetComponent<Card>();

			card.SetUsedStatus(false);
		}
	}

	private void ShuffleCardsIfNeeded()
	{
		var shouldShuffleCards = GameObject.FindGameObjectsWithTag(tag).Count(o => o.GetComponent<Card>().Used) == CARDS_PER_TURN;

		if(shouldShuffleCards)
		{
			ShuffleCards();
		}
	}

	private void Start()
	{
		m_collider = GetComponent<Collider2D>();
		m_startPosition = transform.position;
	}

	private bool UnStunPlayer()
	{
		if(Player.GetComponent<Player>().Stuned)
		{
			Player.GetComponent<Player>().UnStun();
			return true;
		}

		return false;
	}

	private void Update()
	{
		SetStunStatus();
		//SetCardPositions();

		if(AllAIHaveDoneTheirTurn())
		{
			AI.DoTurn = false;
			ShuffleCardsIfNeeded();
		}

		if(TouchInput.IsTouched(m_collider) || m_clicked)
		{
			m_clicked = false;

			if(AI.DoTurn)
			{
				return;
			}

			if(Used)
			{
				return;
			}

			AI.DoTurn = true;

			if(UnStunPlayer())
			{
				return;
			}

			UseCard();
		}
	}

	private void UseCard()
	{
		DoCardEffect();
		SetUsedStatus(true);
	}
}