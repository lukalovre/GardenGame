using Assets.Code;
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

	private void GenerateGrid(int width, int heigth)
	{
		GameObject.Find("TilemapTerrain").GetComponent<Terrain>().GenerateGrid(width, heigth);

		GameObjectList = new List<GameObject>
		{
			Strawberry,
			Player,
			Snail,
			Snail2,
			Snail3
		};

		foreach(var gameObject in GameObject.FindGameObjectsWithTag(Rock.tag).ToList())
		{
			GameObjectPool.Delete(gameObject);
		}

		Grid = Maze.GenerateMaze(width, heigth);

		var bottomHalf = heigth / 2;
		var safeDistance = 1;

		var emptyTiles = Grid.GetEmptyTiles();

		foreach(var gameObject in GameObjectList)
		{
			var position = emptyTiles.FirstOrDefault();
			emptyTiles.Remove(position);

			gameObject.transform.position = position;
		}

		//// Add Strawberry
		//Strawberry.transform.position = new Vector3(Random.Range(0, width), Random.Range(0, bottomHalf - safeDistance));

		//// Add Player
		//// Player will be on the same spot as the strawberry
		//Player.transform.position = new Vector3(Random.Range(0, width), Random.Range(0, bottomHalf - safeDistance));

		//System.Random rnd = new System.Random();

		//var positionX = Enumerable.Range(0, width).OrderBy(n => n * n * rnd.Next())
		//	.Distinct().Take(3).ToList();

		//var positionY = Enumerable.Range(0, heigth - (bottomHalf + safeDistance)).OrderBy(n => n * n * rnd.Next())
		//	.Distinct().Take(3).Select(e => e + bottomHalf + safeDistance).ToList();

		//// Add Snail
		//Snail.transform.position = new Vector3(positionX[0], positionY[0]);

		//// Add Snail2
		//Snail2.transform.position = new Vector3(positionX[1], positionY[1]);

		//// Add Snail3
		//Snail3.transform.position = new Vector3(positionX[2], positionY[2]);

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

		Snail.GetComponent<AI>().FindPath(Snail.transform.position, Strawberry.transform.position, Grid);
		Snail.GetComponent<AI>().SetLocations();

		Snail2.GetComponent<AI>().FindPath(Snail2.transform.position, Strawberry.transform.position, Grid);
		Snail2.GetComponent<AI>().SetLocations();

		Snail3.GetComponent<AI>().FindPath(Snail3.transform.position, Strawberry.transform.position, Grid);
		Snail3.GetComponent<AI>().SetLocations();
	}

	private void Start()
	{
		GenerateGrid(6, 8);
	}

	private void Update()
	{
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			int width = Random.Range(3, 7);
			int heigth = Random.Range(3, 9);

			GenerateGrid(6, 8);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			int width = Random.Range(3, 7);
			int heigth = Random.Range(3, 9);

			GenerateGrid(6, 8);
		}
	}
}