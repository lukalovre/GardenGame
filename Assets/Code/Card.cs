using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Deck;

public class Card : MonoBehaviour
{
	public GameObject Explosion;
	public GameObject Player;
	public GameObject Snail;
	public GameObject Strawberry;
	public CardType Type;
	private Collider2D m_collider;
	private bool m_mouseClicked;
	private Vector3 m_startPosition;
	public bool Used { get; private set; }

	public void SetUsedStatus(bool used)
	{
		Used = used;
		GetComponent<SpriteRenderer>().color = used ? Color.black : Color.white;
		GetComponent<Rigidbody2D>().bodyType = used ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
		GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);

		if(!used)
		{
			transform.position = m_startPosition;
		}
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

			case CardType.Switch:
				Switch();
				break;

			default:
				break;
		}
	}

	private void FireHeight()
	{
		var validPositions = new List<Vector3>();

		validPositions.AddRange(GetLocationsToRock(new Vector3(0, 1)));
		validPositions.AddRange(GetLocationsToRock(new Vector3(0, -1)));

		foreach(var position in validPositions)
		{
			SetExplosion(position);
		}
	}

	private void FireNeighbours()
	{
		var validNeighbours = GenerateMap.Grid.GetValidNeighbors(Player.transform.position);

		foreach(var neighbour in validNeighbours)
		{
			SetExplosion(neighbour);
		}
	}

	private void FireWidth()
	{
		var validPositions = new List<Vector3>();

		validPositions.AddRange(GetLocationsToRock(new Vector3(-1, 0)));
		validPositions.AddRange(GetLocationsToRock(new Vector3(1, 0)));

		foreach(var position in validPositions)
		{
			SetExplosion(position);
		}
	}

	private List<Vector3> GetLocationsToRock(Vector3 direction)
	{
		var validPositions = new List<Vector3>();

		var maxLenght = Mathf.Max(GenerateMap.Grid.GetLength(0), GenerateMap.Grid.GetLength(1));

		for(int i = 1; i < maxLenght; i++)
		{
			var forwardMovement = Player.transform.position + direction * i;

			if(!GenerateMap.Grid.IsInRange(forwardMovement))
			{
				break;
			}

			int x = (int)forwardMovement.x;
			int y = (int)forwardMovement.y;

			if(GenerateMap.Grid[x, y])
			{
				break;
			}

			validPositions.Add(forwardMovement);
		}

		return validPositions;
	}

	private void Move(Vector3 vector3)
	{
		if(GetNotAllowedDirections(Player.transform.position).Contains(Type))
		{
			return;
		}

		Player.GetComponent<Player>().NextLocation = Player.transform.position + vector3;
	}

	private void OnMouseDown()
	{
		m_mouseClicked = true;
	}

	private void RemoveExplosions()
	{
		GameObject.FindGameObjectsWithTag(Explosion.tag).ToList().ForEach(o => GameObjectPool.Delete(o));
	}

	private void SetExplosion(Vector3 neighbour)
	{
		var explosion = GameObjectPool.Get(Explosion);
		explosion.transform.position = neighbour;
		explosion.GetComponent<Animator>().Play(Explosion.tag, -1, 0);
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

	private void Start()
	{
		m_collider = GetComponent<Collider2D>();
		m_startPosition = transform.position;
	}

	private void Switch()
	{
		var playerPosition = Player.transform.position;
		var strawberryPosition = Strawberry.transform.position;

		Player.transform.position = strawberryPosition;
		Player.GetComponent<Player>().NextLocation = strawberryPosition;

		Strawberry.transform.position = playerPosition;

		foreach(var snail in GameObject.FindGameObjectsWithTag(Snail.tag).Select(o => o.GetComponent<AI>()))
		{
			snail.CalculatePathToStrawberry();
		}
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
		if(GenerateMap.LevelEnding)
		{
			return;
		}

		SetStunStatus();

		if(TouchInput.IsTouched(m_collider) || m_mouseClicked)
		{
			m_mouseClicked = false;

			if(AI.DoTurn)
			{
				return;
			}

			if(Used)
			{
				return;
			}

			RemoveExplosions();

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