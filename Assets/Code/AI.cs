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
	public GameObject NextPath;
	public GameObject Slimeball;
	public GameObject Trail;
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

	public void FindPath(Vector3 end)
	{
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
			m_path.Enqueue(location.Value);
		}
	}

	public void Load()
	{
		GameObject.FindGameObjectsWithTag(Trail.tag).ToList().ForEach(GameObjectPool.Delete);
		m_path = new Queue<Vector3>();

		FindPath(GameObject.Find("Strawberry").transform.position);
		SetNextLocations();
	}

	public void SetNextLocations()
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

	private bool ArrivedAtNextLocation()
	{
		return Vector3.Distance(transform.position, NextLocation.Value) <= 0f;
	}

	private bool ArrivedAtStrawberry()
	{
		return NextLocation == null;
	}

	private void EatStrawberry()
	{
		var neighbours = GenerateMap.Grid.GetValidNeighbors(transform.position);

		if(neighbours.Contains(GameObject.Find("Strawberry").transform.position))
		{
			GameObject.Find("Strawberry").GetComponent<Strawberry>().Bite();
		}
	}

	private void GoToNextLocation()
	{
		transform.position = Vector2.MoveTowards(transform.position,
								NextLocation.Value,
								GlobalSettings.MOVEMENT_SPEED * Time.deltaTime);
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

			var objectInSight = GenerateMap.GameObjectList
				.FirstOrDefault(o => o.transform.position == forwardMovement);

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
		if(ArrivedAtStrawberry())
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
			SetNextActionPosition(NextLocation);
		}

		if(m_selectedAction == Actions.Shoot)
		{
			SetNextActionPosition(CurrentLocaton + m_directionToPlayer);
		}
	}

	private void SetNextActionPosition(Vector3? position)
	{
		if(position == null)
		{
			return;
		}

		m_nextActionIndicator.transform.position = Vector2.Lerp(CurrentLocaton, position.Value, 0.5f);
		m_nextActionIndicator.transform.rotation = SetRotation(position.Value);
		m_nextActionIndicator.GetComponent<SpriteRenderer>().color = new Color(m_color.r, m_color.g, m_color.b, TRAIL_OPACITY);
	}

	private Quaternion SetRotation(Vector3 position)
	{
		if(CurrentLocaton.x != position.x)
		{
			return Quaternion.Euler(0, 0, 90);
		}
		else
		{
			return Quaternion.Euler(0, 0, 0);
		}
	}

	private void SetTrail()
	{
		var trail = GameObjectPool.Create(Trail);

		trail.transform.position = Vector2.Lerp(CurrentLocaton, NextLocation.Value, 0.5f);
		trail.transform.rotation = SetRotation(NextLocation.Value);
		trail.GetComponent<SpriteRenderer>().color = new Color(m_color.r, m_color.g, m_color.b, TRAIL_OPACITY);
	}

	private void Shoot()
	{
		var slimeball = GameObjectPool.Create(Slimeball);
		var direction = (GameObject.Find("Player").transform.position - transform.position).normalized;
		slimeball.GetComponent<SpriteRenderer>().color = m_color;

		slimeball.GetComponent<Slimeball>().Fire(transform.position, direction);
	}

	// Start is called before the first frame update
	private void Start()
	{
		m_nextActionIndicator = Instantiate(NextPath);
		m_color = GetComponent<SpriteRenderer>().color;
		m_pathfindingAlgorithm = (Pathfinding)Random.Range(0, 3);
	}

	// Update is called once per frame
	private void Update()
	{
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
			EatStrawberry();
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
				SetNextLocations();
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