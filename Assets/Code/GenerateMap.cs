﻿using Assets.Code;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	public static GridObject[,] MapMatrix;
	public GameObject Player;
	public GameObject Snail;
	public GameObject Strawberry;

	private void GenerateGrid(int columns, int rows)
	{
		MapMatrix = new GridObject[columns, rows];

		// Make empty grid
		for(int y = 0; y < rows; y++)
		{
			for(int x = 0; x < columns; x++)
			{
				MapMatrix[x, y] = new GridObject
				{
					Position = new Vector3(x, y),
					ObjectType = GridObject.Type.Empty
				};
			}
		}

		var bottomHalf = rows / 2;
		var safeDistance = 1;

		// Add Strawberry
		var m = MapMatrix[Random.Range(0, columns), Random.Range(0, bottomHalf - safeDistance)];
		var p = m.Position;
		m.GameObject = Strawberry;
		m.ObjectType = GridObject.Type.Snail;
		Strawberry.transform.position = new Vector3(p.x, p.y);

		// Add Player
		// Player will be on the same spot as the strawberry
		m = MapMatrix[Random.Range(0, columns), Random.Range(0, bottomHalf - safeDistance)];
		p = m.Position;
		m.GameObject = Player;
		m.ObjectType = GridObject.Type.Player;
		Player.transform.position = new Vector3(p.x, p.y);
		Player.GetComponent<Player>().NextLocation = Player.transform.position;

		// Add Snail
		m = MapMatrix[Random.Range(0, columns), Random.Range(bottomHalf + safeDistance, rows)];
		p = m.Position;
		m.GameObject = Snail;
		m.ObjectType = GridObject.Type.Snail;
		Snail.transform.position = new Vector3(p.x, p.y);

		// Add Rocks
		for(int y = 0; y < rows; y++)
		{
			for(int x = 0; x < columns; x++)
			{
				if(MapMatrix[x, y].ObjectType != GridObject.Type.Empty)
				{
					continue;
				}

				MapMatrix[x, y].ObjectType = GridObject.Type.Rock;
				MapMatrix[x, y].Create();
			}
		}
	}

	private void Start()
	{
		GenerateGrid(6, 8);
	}

	private void Update()
	{
	}
}