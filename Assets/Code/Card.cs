using Assets.Code;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
	public GameObject Player;
	public CardType Type;
	private Collider2D collider;

	private bool m_clicked;
	private Vector3 m_startPosition;

	public enum CardType
	{
		Up,
		Down,
		Left,
		Right
	}

	private bool IsTouched()
	{
		if(Input.touchCount == 0)
		{
			return false;
		}

		var touch = Input.GetTouch(0);
		var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
		var touchedCollider = Physics2D.OverlapPoint(touchPosition);
		return collider == touchedCollider && touch.phase == TouchPhase.Began;
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

	private void Start()
	{
		collider = GetComponent<Collider2D>();
		m_startPosition = transform.position;
	}

	private void Update()
	{
		if(Player.GetComponent<Player>().Stuned)
		{
			GetComponent<SpriteRenderer>().color = Player.GetComponent<SpriteRenderer>().color;
		}
		else
		{
			GetComponent<SpriteRenderer>().color = Color.white;
		}

		SetCardPositions();

		if(GameObject.FindGameObjectsWithTag("AI").All(ai => ai.GetComponent<AI>().DoneTurn))
		{
			AI.DoTurn = false;
		}

		if(IsTouched() || m_clicked)
		{
			m_clicked = false;

			if(AI.DoTurn)
			{
				return;
			}

			AI.DoTurn = true;

			if(Player.GetComponent<Player>().Stuned)
			{
				Player.GetComponent<Player>().UnStun();
				return;
			}

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