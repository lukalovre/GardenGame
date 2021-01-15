using Assets.Code;
using Assets.Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour, ILoad
{
	public static bool DoTurn;
	public Vector3 CurrentLocaton;
	public Vector3? m_directionToPlayer;
	public GameObject nextActionIndicator;
	public RuntimeAnimatorController NextMoveAnimation;
	public RuntimeAnimatorController NextShootAnimation;
	public GameObject Slimeball;
	public GameObject Strawberry;
	public GameObject Trail;
	private const float NEXT_ACTION_INDICATOR_OPACITY = 0.8f;
	private const float TRAIL_OPACITY = 0.5f;
	private Color m_color;
	private GameObject m_nextActionIndicator;
	private Queue<Vector3> m_path;
	private Pathfinding m_pathfindingAlgorithm;
	private Actions m_selectedAction;
	private Vector3? NextLocation;

	public enum Actions
	{
		None,
		Move,
		Shoot,
		Eat
	}

	public enum Pathfinding
	{
		DepthFirst,
		AStar,
		UniformCost
	}

	public bool DoneTurn { get; private set; }
	public bool IsDead { get; private set; }

	public void CalculatePathToStrawberry()
	{
		if(IsDead)
		{
			return;
		}

		m_path = FindPath(Strawberry.transform.position);
		SetNextLocation();
	}

	public Queue<Vector3> FindPath(Vector3 end)
	{
		var result = new Queue<Vector3>();
		var path = new List<Vector3?>();
		var start = transform.position;

		switch(m_pathfindingAlgorithm)
		{
			case Pathfinding.DepthFirst:
				path = DepthFirstSearch.GetPath(start, end, GenerateMap.Grid);
				break;

			case Pathfinding.AStar:
				path = AStarSearch.GetPath(start, end, GenerateMap.Grid);
				break;

			case Pathfinding.UniformCost:
				path = UniformCostSearch.GetPath(start, end, GenerateMap.Grid);
				break;

			default:
				break;
		}

		path.Remove(start);
		path.Remove(end);

		foreach(var location in path)
		{
			result.Enqueue(location.Value);
		}

		return result;
	}

	public void Load()
	{
		IsDead = false;
		GameObject.FindGameObjectsWithTag(Trail.tag).ToList().ForEach(GameObjectPool.Delete);
		m_path = new Queue<Vector3>();
		CalculatePathToStrawberry();
	}

	public void SetNextLocation()
	{
		CurrentLocaton = new Vector3(transform.position.x, transform.position.y);

		if(m_path.Count != 0)
		{
			NextLocation = m_path.Dequeue();
		}
		else
		{
			NextLocation = null;
		}
	}

	internal void Die()
	{
		IsDead = true;
		DoneTurn = true;
		transform.position = GameObjectPool.PoolLocation;
		m_nextActionIndicator.transform.position = GameObjectPool.PoolLocation;
	}

	private bool ArrivedAtNextLocation()
	{
		return Vector3.Distance(transform.position, NextLocation.Value) <= 0f;
	}

	private void EatSnack()
	{
		var neighbours = GetNeighbouringObjects();

		var foundSnack = neighbours.FirstOrDefault(o => o.GetComponent<Snack>() != null);

		if(foundSnack != null)
		{
			var snack = foundSnack.GetComponent<Snack>();

			if(snack == null)
			{
				return;
			}

			snack.Bite();

			if(snack.IsDead() && snack.name != Strawberry.name)
			{
				CalculatePathToStrawberry();
			}
		}
	}

	private List<GameObject> GetNeighbouringObjects()
	{
		var result = new List<GameObject>();

		foreach(var direction in GridExtensions.Directions)
		{
			var neighbourObject = GenerateMap.GameObjectList.FirstOrDefault(o => o.transform.position == CurrentLocaton + direction);

			if(neighbourObject == null)
			{
				continue;
			}

			result.Add(neighbourObject);
		}

		return result;
	}

	private void GoToNextLocation()
	{
		transform.position = Vector2.MoveTowards(transform.position,
								NextLocation.Value,
								GlobalSettings.MOVEMENT_SPEED * Time.deltaTime);
	}

	private bool NextToSnack()
	{
		var neighbours = GetNeighbouringObjects();

		return neighbours.Any(o => o.GetComponent<Snack>() != null);
	}

	private bool PlayerInLineOfSight(Vector3 direction)
	{
		var maxLenght = Mathf.Max(GenerateMap.Grid.GetLength(0), GenerateMap.Grid.GetLength(1));

		for(int i = 1; i < maxLenght; i++)
		{
			var forwardMovement = CurrentLocaton + direction * i;

			if(!GenerateMap.Grid.IsInRange(forwardMovement))
			{
				return false;
			}

			int x = (int)forwardMovement.x;
			int y = (int)forwardMovement.y;

			if(GenerateMap.Grid[x, y])
			{
				return false;
			}

			var objectInSight = GenerateMap.GameObjectList.FirstOrDefault(o => o.transform.position == forwardMovement);

			if(objectInSight == null)
			{
				continue;
			}

			var player = objectInSight.GetComponent<Player>();

			if(player == null)
			{
				return false;
			}

			if(!player.Stuned)
			{
				return true;
			}
		}

		return false;
	}

	private bool PlayerVisible()
	{
		foreach(var direction in GridExtensions.Directions)
		{
			if(PlayerInLineOfSight(direction))
			{
				m_directionToPlayer = direction;
				return true;
			}
		}

		return false;
	}

	private Actions SelectTurnAction()
	{
		if(NextToSnack())
		{
			return Actions.Eat;
		}

		if(PlayerVisible())
		{
			return Actions.Shoot;
		}

		return Actions.Move;
	}

	private void SetNextAction()
	{
		if(m_selectedAction == Actions.Move)
		{
			SetNextActionPosition(NextLocation, NextMoveAnimation);
			return;
		}

		if(m_selectedAction == Actions.Shoot)
		{
			SetNextActionPosition(CurrentLocaton + m_directionToPlayer, NextShootAnimation);
			return;
		}

		m_nextActionIndicator.transform.position = GameObjectPool.PoolLocation;
	}

	private void SetNextActionPosition(Vector3? position, RuntimeAnimatorController animation)
	{
		if(position == null)
		{
			return;
		}

		m_nextActionIndicator.transform.position = Vector2.Lerp(CurrentLocaton, position.Value, 0.5f);
		m_nextActionIndicator.transform.rotation = SetRotation(position.Value);
		m_nextActionIndicator.GetComponent<SpriteRenderer>().color = new Color(m_color.r, m_color.g, m_color.b, NEXT_ACTION_INDICATOR_OPACITY);
		m_nextActionIndicator.GetComponent<Animator>().runtimeAnimatorController = animation;
	}

	private Quaternion SetRotation(Vector3 position)
	{
		var angle = Mathf.Atan2(position.y - CurrentLocaton.y, position.x - CurrentLocaton.x);
		var addstartingAngle = 90;

		return Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle + addstartingAngle);
	}

	private void SetTrail()
	{
		var trail = GameObjectPool.Get(Trail);

		trail.transform.position = Vector2.Lerp(CurrentLocaton, NextLocation.Value, 0.5f);
		trail.transform.rotation = SetRotation(NextLocation.Value);
		trail.GetComponent<SpriteRenderer>().color = new Color(m_color.r, m_color.g, m_color.b, TRAIL_OPACITY);
	}

	private void Shoot()
	{
		var slimeball = GameObjectPool.Get(Slimeball);
		slimeball.GetComponent<SpriteRenderer>().color = m_color;

		slimeball.GetComponent<Slimeball>().Fire(transform.position, m_directionToPlayer.Value);
	}

	private void Start()
	{
		m_nextActionIndicator = GameObjectPool.Get(nextActionIndicator);
		m_color = GetComponent<SpriteRenderer>().color;
		m_pathfindingAlgorithm = (Pathfinding)Random.Range(0, 3);
	}

	private void Update()
	{
		if(IsDead)
		{
			return;
		}

		if(!DoTurn)
		{
			DoneTurn = false;
			m_selectedAction = SelectTurnAction();
			SetNextAction();
			return;
		}

		if(DoneTurn)
		{
			return;
		}

		if(m_selectedAction == Actions.Eat)
		{
			EatSnack();
			DoneTurn = true;
			return;
		}

		if(m_selectedAction == Actions.Shoot)
		{
			Shoot();
			DoneTurn = true;
			return;
		}

		if(m_selectedAction == Actions.Move)
		{
			if(NextLocation == null)
			{
				DoneTurn = true;
				return;
			}

			if(ArrivedAtNextLocation())
			{
				SetTrail();
				SetNextLocation();
				DoneTurn = true;
				return;
			}
			else
			{
				GoToNextLocation();
			}
		}
	}
}