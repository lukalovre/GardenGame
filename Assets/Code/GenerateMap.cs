﻿using Assets.Code;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	public static GridObject[,] MapMatrix;
	public GameObject Player;
	public GameObject Snail;
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

		// Add Snail
		m = MapMatrix[Random.Range(0, width), Random.Range(bottomHalf + safeDistance, heigth)];
		p = m.Position;
		m.GameObject = Snail;
		m.ObjectType = GridObject.Type.Snail;
		Snail.transform.position = new Vector3(p.x, p.y);
		Snail.GetComponent<AI>().SetLocations();

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

				if(grid[x, y])
				{
					MapMatrix[x, y].ObjectType = GridObject.Type.Rock;
					MapMatrix[x, y].Create();
				}
			}
		}
	}

	private void Start()
	{
		GenerateGrid(5, 7);
	}

	private void Update()
	{
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			int width = Random.Range(3, 7);
			int heigth = Random.Range(3, 9);

			GenerateGrid(5, 7);
		}
	}
}