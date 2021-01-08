using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateMap : MonoBehaviour
{
	public TileBase grass;
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

				tilemap.SetTile(new Vector3Int(x, y, 0), tile);
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