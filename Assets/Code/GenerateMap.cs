using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	public static List<GameObject> GameObjectList;
	public static bool[,] Grid;
	public static Vector3 StrawberryPosition;
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

		var emptyTiles = Grid.GetEmptyTiles();

		var bottomHalf = heigth / 2;
		var safeDistance = 1;

		var bottomHalfEmptyTiles = emptyTiles.Where(tile => tile.y <= bottomHalf - safeDistance).ToList();
		var topHalfEmptyTiles = emptyTiles.Where(tile => tile.y >= bottomHalf + safeDistance).ToList();

		foreach(var gameObject in GameObjectList)
		{
			Vector3 position;

			if(gameObject.CompareTag(Snail.tag))
			{
				position = topHalfEmptyTiles.FirstOrDefault();
				topHalfEmptyTiles.Remove(position);
			}
			else
			{
				position = bottomHalfEmptyTiles.FirstOrDefault();
				bottomHalfEmptyTiles.Remove(position);
			}

			gameObject.transform.position = position;
		}

		StrawberryPosition = Strawberry.transform.position;

		foreach(var gameObject in GameObjectList)
		{
			gameObject.GetComponent<ILoad>()?.Load();
		}

		//System.Random rnd = new System.Random();

		//var positionX = Enumerable.Range(0, width).OrderBy(n => n * n * rnd.Next())
		//	.Distinct().Take(3).ToList();

		//var positionY = Enumerable.Range(0, heigth - (bottomHalf + safeDistance)).OrderBy(n => n * n * rnd.Next())
		//	.Distinct().Take(3).Select(e => e + bottomHalf + safeDistance).ToList();

		// Add Rocks
		for(int y = 0; y < heigth; y++)
		{
			for(int x = 0; x < width; x++)
			{
				if(Grid[x, y] /*&& Random.Range(1, 7) != 1*/)
				{
					var rock = GameObjectPool.Create(Rock);
					rock.transform.position = new Vector3(x, y);
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