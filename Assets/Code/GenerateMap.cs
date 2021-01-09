using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	public static List<GridObject> GridObjectList;

	public GameObject Player;

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

				var diceRoll = Random.Range(1, 13);

				switch(diceRoll)
				{
					case 4:
					case 5:
						gridObject.ObjectType = GridObject.Type.Rock;
						break;

					case 6:
						gridObject.ObjectType = GridObject.Type.Ant;
						break;

					default:
						gridObject.ObjectType = GridObject.Type.Empty;
						break;
				}

				GridObjectList.Add(gridObject);
			}
		}

		foreach(var gridObject1 in GridObjectList)
		{
			gridObject1.Create();
		}

		var p = GridObjectList.FirstOrDefault(o => o.ObjectType == GridObject.Type.Empty).Position;
		Player.transform.position = new Vector3(p.x, p.y, 0);
	}

	private void Start()
	{
		GenerateGrid(6, 8);
	}

	private void Update()
	{
	}
}