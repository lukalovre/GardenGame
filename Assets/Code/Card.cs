using Assets.Code;
using Assets.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
	public GameObject Player;
	public Sprite SpriteDown;
	public Sprite SpriteFireHeight;
	public Sprite SpriteFireNeighbours;
	public Sprite SpriteFireWidth;
	public Sprite SpriteLeft;
	public Sprite SpriteRight;
	public Sprite SpriteUp;
	public CardType Type;
	private const int CARDS_PER_TURN = 2;
	private const int NUMBER_OF_CARDS = 4;
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

	private Sprite GetCardImage(CardType type)
	{
		switch(type)
		{
			case CardType.Up:
				return SpriteUp;

			case CardType.Down:
				return SpriteDown;

			case CardType.Left:
				return SpriteLeft;

			case CardType.Right:
				return SpriteRight;

			case CardType.FireNeighbours:
				return SpriteFireNeighbours;

			case CardType.FireWidth:
				return SpriteFireWidth;

			case CardType.FireHeight:
				return SpriteFireHeight;

			default:
				return null;
		}
	}

	private List<CardType> GetNotAllowedDirections()
	{
		var result = new List<CardType>();

		foreach(var direction in GridExtensions.Directions)
		{
			var validNextPositions = GenerateMap.Grid.GetValidNeighbors(Player.transform.position);

			if(validNextPositions.Contains(Player.transform.position + direction))
			{
				continue;
			}

			if(direction == new Vector3(0, 1))
			{
				result.Add(CardType.Up);
			}

			if(direction == new Vector3(0, -1))
			{
				result.Add(CardType.Down);
			}

			if(direction == new Vector3(-1, 0))
			{
				result.Add(CardType.Left);
			}

			if(direction == new Vector3(1, 0))
			{
				result.Add(CardType.Right);
			}
		}

		return result;
	}

	private List<int> GetRandomCardType()
	{
		var numberOfCardTypes = System.Enum.GetNames(typeof(CardType)).Length;
		var enumNumberList = Enumerable.Range(0, numberOfCardTypes).ToList();

		foreach(var notAllowedDirection in GetNotAllowedDirections())
		{
			enumNumberList.Remove((int)notAllowedDirection);
		}

		var numberList = new List<int>();
		numberList.AddRange(enumNumberList);
		numberList.AddRange(enumNumberList);

		return Helper.Shuffle(numberList).Take(NUMBER_OF_CARDS).ToList();
	}

	private void Move(Vector3 vector3)
	{
		Player.GetComponent<Player>().NextLocation = Player.transform.position + vector3;
	}

	private void OnMouseDown()
	{
		m_clicked = true;
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
		var randomCardTypes = new Stack(GetRandomCardType());

		foreach(var card in GameObject.FindGameObjectsWithTag(tag).ToList().Select(o => o.GetComponent<Card>()))
		{
			card.SetUsedStatus(false);
			card.Type = (CardType)randomCardTypes.Pop();
			card.GetComponent<SpriteRenderer>().sprite = GetCardImage(card.Type);
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
		ShuffleCards();
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