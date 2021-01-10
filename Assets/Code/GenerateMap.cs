using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	public static List<GridObject> GridObjectList;

	public static GridObject[,] MapMatrix;
	public GameObject Ant;
	public GameObject Player;
	public GameObject Strawberry;

	private void GenerateGrid(int columns, int rows)
	{
		GridObjectList = new List<GridObject>(columns * rows);
		MapMatrix = new GridObject[columns, rows];

		for(int y = 0; y < rows; y++)
		{
			for(int x = 0; x < columns; x++)
			{
				GridObject gridObject = new GridObject
				{
					Position = new Vector3(x, y)
				};

				var diceRoll = Random.Range(1, 13);

				switch(diceRoll)
				{
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
						gridObject.ObjectType = GridObject.Type.Rock;
						break;

					default:
						gridObject.ObjectType = GridObject.Type.Empty;
						break;
				}

				GridObjectList.Add(gridObject);
				MapMatrix[x, y] = gridObject;
			}
		}

		foreach(var gridObject1 in GridObjectList)
		{
			gridObject1.Create();
		}

		var p = GridObjectList.FirstOrDefault(o => o.ObjectType == GridObject.Type.Empty).Position;
		Player.transform.position = new Vector3(p.x, p.y);
		Player.GetComponent<Player>().NextLocation = Player.transform.position;

		p = GridObjectList.LastOrDefault(o => o.ObjectType == GridObject.Type.Empty).Position;
		Ant.transform.position = new Vector3(p.x, p.y);

		p = GridObjectList.Where(o => o.ObjectType == GridObject.Type.Empty).ElementAt(2).Position;
		Strawberry.transform.position = new Vector3(p.x, p.y);
	}

	private void Start()
	{
		GenerateGrid(6, 8);
	}

	private void Update()
	{
	}
}