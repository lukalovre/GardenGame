using Assets.Code;
using Assets.Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour, ILoad
{
	public static bool DoTurn;
	public Vector3 CurrentLocaton;
	public GameObject NextPath;
	public GameObject Slimeball;
	public GameObject Trail;
	private const float TRAIL_OPACITY = 0.5f;
	private Collider2D collider;
	private Color m_color;
	private GameObject m_nextPath;
	private Queue<Vector3> m_path;
	private Pathfinding m_pathfindingAlgorithm;
	private GameObject m_slimeball;
	private Vector3? NextLocation;

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

	private void OnTriggerEnter2D(Collider2D collision)
	{
	}

	private bool PlayerInLineOfSight()
	{
		var nextMovementChange = NextLocation.Value - CurrentLocaton;

		var maxLenght = Mathf.Max(GenerateMap.Grid.GetLength(0), GenerateMap.Grid.GetLength(1));

		for(int i = 1; i < maxLenght; i++)
		{
			var forwardMovement = CurrentLocaton + nextMovementChange * i;

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

			var player = GameObject.Find("Player");

			if(forwardMovement == player.transform.position)
			{
				return true;
			}
		}

		return false;
	}

	private bool PlayerVisible()
	{
		return false;
	}

	private void SetNextLocationPath()
	{
		if(NextLocation == null)
		{
			m_nextPath.transform.position = GameObjectPool.PoolLocation;
			return;
		}

		m_nextPath.transform.position = Vector2.Lerp(CurrentLocaton, NextLocation.Value, 0.5f);
		m_nextPath.transform.rotation = SetRotation();
	}

	private Quaternion SetRotation()
	{
		if(CurrentLocaton.x != NextLocation.Value.x)
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
		trail.transform.rotation = SetRotation();
		trail.GetComponent<SpriteRenderer>().color = new Color(m_color.r, m_color.g, m_color.b, TRAIL_OPACITY);
	}

	private void Shoot()
	{
	}

	// Start is called before the first frame update
	private void Start()
	{
		collider = GetComponent<Collider2D>();
		m_nextPath = Instantiate(NextPath);
		m_color = GetComponent<SpriteRenderer>().color;
		m_pathfindingAlgorithm = (Pathfinding)Random.Range(0, 3);
		m_slimeball = GameObjectPool.Create(Slimeball);
	}

	private void TurnToPlayer()
	{
	}

	// Update is called once per frame
	private void Update()
	{
		if(!DoTurn)
		{
			SetNextLocationPath();
			DoneTurn = false;
			return;
		}

		if(DoneTurn)
		{
			return;
		}

		if(ArrivedAtStrawberry())
		{
			EatStrawberry();
			DoneTurn = true;
			return;
		}

		if(PlayerInLineOfSight())
		{
			Shoot();
			DoneTurn = true;
			return;
		}

		if(PlayerVisible())
		{
			TurnToPlayer();
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