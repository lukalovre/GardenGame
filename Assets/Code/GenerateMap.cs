﻿using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateMap : MonoBehaviour
{
	public TileBase ant;
	public TileBase rock;
	public Tilemap tilemap;

	private void GenerateGrid(int columns, int rows)
	{
		tilemap.ClearAllTiles();

		for(int y = 0; y < rows; y++)
		{
			for(int x = 0; x < columns; x++)
			{
				var tile = rock;

				if(Random.Range(1, 7) == 1)
				{
					tilemap.SetTile(new Vector3Int(x, y, 0), tile);
				}

				if(Random.Range(1, 24) == 1)
				{
					tilemap.SetTile(new Vector3Int(x, y, 0), ant);
				}
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