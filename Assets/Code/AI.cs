using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour
{
	public static bool DoTurn;
	public Vector3 CurrentLocaton;
	private readonly Queue<Vector3> Path = new Queue<Vector3>();
	private Collider2D collider;
	private GameObject m_path;
	private GameObject m_trail;
	private Vector3 NextLocation;
	public bool DoneTurn { get; private set; }

	public void FindPath(Vector3 start, Vector3 destination)
	{
		var p = GetRandomDirection(start);
		Path.Enqueue(p);

		var p1 = GetRandomDirection(p);
		Path.Enqueue(p1);
	}

	public void SetLocations()
	{
		CurrentLocaton = new Vector3(transform.position.x, transform.position.y);

		if(Path.Count != 0)
		{
			NextLocation = Path.Dequeue();
		}
	}

	private Vector3 GetRandomDirection(Vector3 currentPosition)
	{
		int x = (int)currentPosition.x;
		int y = (int)currentPosition.y;

		var gridObject = GenerateMap.MapMatrix[x, y];

		var validNextPositions = gridObject?.GetValidMoveLocations();
		var randonPositionIndex = Random.Range(0, validNextPositions.Count);

		var newPosition = validNextPositions?.ElementAt(randonPositionIndex);

		if(newPosition == null)
		{
			return transform.position;
		}

		return newPosition.Value;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
	}

	private void SetNextLocationPath()
	{
		m_path.transform.position = Vector2.Lerp(CurrentLocaton, NextLocation, 0.5f);
		m_path.transform.rotation = SetRotation();
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
		var trail = GameObject.Instantiate(m_trail);

		trail.transform.position = Vector2.Lerp(CurrentLocaton, NextLocation, 0.5f);
		trail.transform.rotation = SetRotation();
	}

	// Start is called before the first frame update
	private void Start()
	{
		collider = GetComponent<Collider2D>();
		m_path = Instantiate(GameObject.Find("Path"));
		m_trail = Instantiate(GameObject.Find("Trail"));
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