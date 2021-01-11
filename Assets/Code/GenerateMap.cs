using Assets.Code;
using System.Linq;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	public static GridObject[,] MapMatrix;
	public GameObject Player;
	public GameObject Snail;
	public GameObject Snail2;
	public GameObject Snail3;
	public GameObject Strawberry;

	private void GenerateGrid(int width, int heigth)
	{
		GameObject.Find("TilemapTerrain").GetComponent<Terrain>().GenerateGrid(width, heigth);

		if(MapMatrix != null)
		{
			foreach(var gridObject in MapMatrix)
			{
				if(gridObject.GameObject != null && gridObject.ObjectType == GridObject.Type.Rock)
				{
					Destroy(gridObject.GameObject);
				}
			}
		}

		MapMatrix = new GridObject[width, heigth];

		// Make empty grid
		for(int y = 0; y < heigth; y++)
		{
			for(int x = 0; x < width; x++)
			{
				MapMatrix[x, y] = new GridObject
				{
					Position = new Vector3(x, y),
					ObjectType = GridObject.Type.Empty
				};
			}
		}

		var bottomHalf = heigth / 2;
		var safeDistance = 1;

		// Add Strawberry
		var m = MapMatrix[Random.Range(0, width), Random.Range(0, bottomHalf - safeDistance)];
		var p = m.Position;
		m.GameObject = Strawberry;
		m.ObjectType = GridObject.Type.Snail;
		Strawberry.transform.position = new Vector3(p.x, p.y);

		// Add Player
		// Player will be on the same spot as the strawberry
		m = MapMatrix[Random.Range(0, width), Random.Range(0, bottomHalf - safeDistance)];
		p = m.Position;
		m.GameObject = Player;
		m.ObjectType = GridObject.Type.Player;
		Player.transform.position = new Vector3(p.x, p.y);
		Player.GetComponent<Player>().NextLocation = Player.transform.position;

		System.Random rnd = new System.Random();

		var positionX = Enumerable.Range(0, width).OrderBy(n => n * n * rnd.Next())
			.Distinct().Take(3).ToList();

		var positionY = Enumerable.Range(0, heigth - (bottomHalf + safeDistance)).OrderBy(n => n * n * rnd.Next())
			.Distinct().Take(3).Select(e => e + bottomHalf + safeDistance).ToList();

		// Add Snail
		m = MapMatrix[positionX[0], positionY[0]];
		p = m.Position;
		m.GameObject = Snail;
		m.ObjectType = GridObject.Type.Snail;
		Snail.transform.position = new Vector3(p.x, p.y);

		// Add Snail2
		Snail2.transform.position = new Vector3(positionX[1], positionY[1]);

		// Add Snail3
		Snail3.transform.position = new Vector3(positionX[2], positionY[2]);

		var grid = Maze.GenerateMaze(width, heigth);

		// Add Rocks
		for(int y = 0; y < heigth; y++)
		{
			for(int x = 0; x < width; x++)
			{
				if(MapMatrix[x, y].ObjectType != GridObject.Type.Empty)
				{
					continue;
				}

				if(grid[x, y] && Random.Range(1, 7) != 1)
				{
					MapMatrix[x, y].ObjectType = GridObject.Type.Rock;
					MapMatrix[x, y].Create();
				}
			}
		}

		Snail.GetComponent<AI>().FindPath(Snail.transform.position, Strawberry.transform.position, grid);
		Snail.GetComponent<AI>().SetLocations();

		Snail2.GetComponent<AI>().FindPath(Snail2.transform.position, Strawberry.transform.position, grid);
		Snail2.GetComponent<AI>().SetLocations();

		Snail3.GetComponent<AI>().FindPath(Snail3.transform.position, Strawberry.transform.position, grid);
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
	}
}