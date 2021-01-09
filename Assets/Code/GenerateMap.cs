using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	public static List<GridObject> GridObjectList;

	private void GenerateGrid(int columns, int rows)
	{
		GridObjectList = new List<GridObject>(columns * rows);

		for(int y = 0; y < rows; y++)
		{
			for(int x = 0; x < columns; x++)
			{
				GridObject gridObject = new GridObject
				{
					Position = new Vector2Int(x, y)
				};

				var diceRoll = Random.Range(1, 7);

				switch(diceRoll)
				{
					case 1:
					case 2:
					case 3:
						gridObject.ObjectType = GridObject.Type.Empty;
						break;

					case 4:
					case 5:
						gridObject.ObjectType = GridObject.Type.Rock;
						break;

					case 6:
						gridObject.ObjectType = GridObject.Type.Ant;
						break;

					default:
						break;
				}

				GridObjectList.Add(gridObject);
			}
		}

		foreach(var gridObject1 in GridObjectList)
		{
			gridObject1.Create();
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