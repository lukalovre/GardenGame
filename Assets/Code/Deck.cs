using Assets.Code;
using Assets.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
	public GameObject Explosion;
	public GameObject Player;
	public Sprite SpriteDown;
	public Sprite SpriteFireHeight;
	public Sprite SpriteFireNeighbours;
	public Sprite SpriteFireWidth;
	public Sprite SpriteLeft;
	public Sprite SpriteRight;
	public Sprite SpriteSwitch;
	public Sprite SpriteUp;
	private const int CARDS_PER_TURN = 2;
	private const int NUMBER_OF_CARDS = 4;

	public enum CardType
	{
		Up,
		Down,
		Left,
		Right,
		FireNeighbours,
		FireWidth,
		FireHeight,
		Switch
	}

	public static List<CardType> GetNotAllowedDirections(Vector3 position)
	{
		var result = new List<CardType>();

		foreach(var direction in GridExtensions.Directions)
		{
			var validNextPositions = GenerateMap.Grid.GetValidNeighbors(position);

			if(validNextPositions.Contains(position + direction))
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

	private static bool TurnIsDone()
	{
		var allAIHaveDoneTheirTurn = GameObject.FindGameObjectsWithTag("AI").All(ai => ai.GetComponent<AI>().DoneTurn);

		return allAIHaveDoneTheirTurn;
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

			case CardType.Switch:
				return SpriteSwitch;

			default:
				return null;
		}
	}

	private List<int> GetRandomCardType()
	{
		var numberOfCardTypes = System.Enum.GetNames(typeof(CardType)).Length;
		var enumNumberList = Enumerable.Range(0, numberOfCardTypes).ToList();

		var numberList = new List<int>();
		numberList.AddRange(enumNumberList);
		numberList.AddRange(enumNumberList);

		return Helper.Shuffle(numberList).Take(NUMBER_OF_CARDS).ToList();
	}

	private void ShuffleCards()
	{
		var randomCardTypes = new Stack(GetRandomCardType());

		foreach(var card in GameObject.FindGameObjectsWithTag("Card").ToList().Select(o => o.GetComponent<Card>()))
		{
			card.SetUsedStatus(false);
			card.Type = (CardType)randomCardTypes.Pop();
			card.GetComponent<SpriteRenderer>().sprite = GetCardImage(card.Type);
		}
	}

	private void ShuffleCardsIfNeeded()
	{
		var shouldShuffleCards = GameObject.FindGameObjectsWithTag("Card").Count(o => o.GetComponent<Card>().Used) == CARDS_PER_TURN;

		if(shouldShuffleCards)
		{
			ShuffleCards();
		}
	}

	private void Start()
	{
		ShuffleCards();
	}

	private void Update()
	{
		if(TurnIsDone())
		{
			AI.DoTurn = false;
			ShuffleCardsIfNeeded();
		}
	}
}