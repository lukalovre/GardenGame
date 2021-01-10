using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour
{
	public static bool DoTurn;
	private Collider2D collider;
	private GameObject m_path;
	private GameObject m_trail;
	private Vector3 NextLocation;
	private List<Vector3> Path = new List<Vector3>();
	private Vector3 StartLocation;
	public bool DoneTurn { get; private set; }

	private Vector3 GetRandomDirection()
	{
		var x = (int)transform.position.x;
		var y = (int)transform.position.y;

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
		m_path.transform.position = Vector2.Lerp(StartLocation, NextLocation, 0.5f);
		m_path.transform.rotation = SetRotation();
	}

	private Quaternion SetRotation()
	{
		if(StartLocation.x != NextLocation.x)
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

		trail.transform.position = Vector2.Lerp(StartLocation, NextLocation, 0.5f);
		trail.transform.rotation = SetRotation();
	}

	// Start is called before the first frame update
	private void Start()
	{
		collider = GetComponent<Collider2D>();
		StartLocation = new Vector3(transform.position.x, transform.position.y);
		NextLocation = GetRandomDirection();

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

			NextLocation = GetRandomDirection();
			StartLocation = new Vector3(transform.position.x, transform.position.y);

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