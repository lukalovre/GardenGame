using Assets.Code;
using Assets.Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	public static List<GameObject> GameObjectList;
	public static bool[,] Grid;
	public GameObject Player;
	public GameObject Rock;
	public GameObject Snail;
	public GameObject Snail2;
	public GameObject Snail3;
	public GameObject Strawberry;
	public GameObject TilemapTerrain;

	private void GenerateGrid(int width, int heigth)
	{
		GameObject.FindGameObjectsWithTag(Rock.tag).ToList().ForEach(GameObjectPool.Delete);

		TilemapTerrain.GetComponent<Terrain>().GenerateGrid(width, heigth);

		GameObjectList = new List<GameObject>
		{
			Strawberry,
			Player,
			Snail,
			Snail2,
			Snail3
		};

		Grid = Maze.GenerateMaze(width, heigth);

		SetGameObjectPositions(width, heigth);

		foreach(var gameObject in GameObjectList)
		{
			gameObject.GetComponent<ILoad>()?.Load();
		}
	}

	private void SetGameObjectPositions(int width, int heigth)
	{
		var bottomHalf = heigth / 2;
		var safeDistance = 1;

		var emptyTiles = Grid.GetEmptyTiles();

		var bottomTiles = emptyTiles.Where(tile => tile.y <= bottomHalf - safeDistance);
		var bottomTilesShuffled = new Stack<Vector3>(Helper.Shuffle(bottomTiles.ToList()));

		var topTiles = emptyTiles.Where(tile => tile.y >= bottomHalf + safeDistance);
		var topTilesShuffled = new Stack<Vector3>(Helper.Shuffle(topTiles.ToList()));

		foreach(var gameObject in GameObjectList)
		{
			Vector3 position;

			if(gameObject.CompareTag(Snail.tag))
			{
				position = topTilesShuffled.Pop();
			}
			else
			{
				position = bottomTilesShuffled.Pop();
			}

			gameObject.transform.position = position;
		}

		// Add Rocks
		for(int y = 0; y < heigth; y++)
		{
			for(int x = 0; x < width; x++)
			{
				if(Grid[x, y] && Random.Range(1, 2) != 1)
				{
					var rock = GameObjectPool.Create(Rock);
					rock.transform.position = new Vector3(x, y);
				}
				else
				{
					Grid[x, y] = false;
				}
			}
		}
	}

	private void Start()
	{
		GenerateGrid(GlobalSettings.MAX_WIDTH, GlobalSettings.MAX_HEIGHT);
	}

	private void Update()
	{
		if(Strawberry.GetComponent<Strawberry>().IsDead()
			&& GameObject.FindGameObjectsWithTag("AI").All(ai => ai.GetComponent<AI>().DoneTurn))
		{
			int width = Random.Range(4, GlobalSettings.MAX_WIDTH);
			int heigth = Random.Range(4, GlobalSettings.MAX_HEIGHT);

			GenerateGrid(width, heigth);
		}

		if(Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			int width = Random.Range(3, 7);
			int heigth = Random.Range(3, 9);

			GenerateGrid(GlobalSettings.MAX_WIDTH, GlobalSettings.MAX_HEIGHT);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			int width = Random.Range(3, 7);
			int heigth = Random.Range(3, 9);

			GenerateGrid(GlobalSettings.MAX_WIDTH, GlobalSettings.MAX_HEIGHT);
		}
	}
}