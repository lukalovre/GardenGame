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
	public GameObject Trail;
	private const float TRAIL_OPACITY = 0.5f;
	private Collider2D collider;
	private Color m_color;
	private GameObject m_nextPath;
	private Queue<Vector3> m_path;
	private Pathfinding m_pathfindingAlgorithm;
	private Vector3 NextLocation;

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

		foreach(var location in path)
		{
			if(location.Value == start)
			{
				continue;
			}

			m_path.Enqueue(location.Value);
		}
	}

	public void Load()
	{
		GameObject.FindGameObjectsWithTag(Trail.tag).ToList().ForEach(GameObjectPool.Delete);
		m_path = new Queue<Vector3>();

		FindPath(GenerateMap.StrawberryPosition);
		SetLocations();
	}

	public void SetLocations()
	{
		CurrentLocaton = new Vector3(transform.position.x, transform.position.y);

		if(m_path.Count != 0)
		{
			NextLocation = m_path.Dequeue();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
	}

	private void SetNextLocationPath()
	{
		m_nextPath.transform.position = Vector2.Lerp(CurrentLocaton, NextLocation, 0.5f);
		m_nextPath.transform.rotation = SetRotation();
	}

	private Quaternion SetRotation()
	{
		if(CurrentLocaton.x != NextLocation.x)
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

		trail.transform.position = Vector2.Lerp(CurrentLocaton, NextLocation, 0.5f);
		trail.transform.rotation = SetRotation();
		trail.GetComponent<SpriteRenderer>().color = new Color(m_color.r, m_color.g, m_color.b, TRAIL_OPACITY);
	}

	// Start is called before the first frame update
	private void Start()
	{
		collider = GetComponent<Collider2D>();
		m_nextPath = Instantiate(NextPath);
		m_color = GetComponent<SpriteRenderer>().color;
		m_pathfindingAlgorithm = (Pathfinding)Random.Range(0, 3);
	}

	// Update is called once per frame
	private void Update()
	{
		if(!DoTurn)
		{
			DoneTurn = false;
			SetNextLocationPath();

			return;
		}

		if(Vector3.Distance(transform.position, NextLocation) <= 0f)
		{
			SetTrail();
			SetLocations();
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