using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	private List<GridObject> m_gridObjectList;

	private void GenerateGrid(int columns, int rows)
	{
		m_gridObjectList = new List<GridObject>(columns * rows);

		for(int y = 0; y < rows; y++)
		{
			for(int x = 0; x < columns; x++)
			{
				GridObject gridObject = new GridObject
				{
					Position = new Vector2Int(x, y)
				};

				if(Random.Range(1, 5) == 1)
				{
					gridObject.ObjectType = GridObject.Type.Rock;
				};

				m_gridObjectList.Add(gridObject);
			}
		}

		foreach(var gridObject1 in m_gridObjectList)
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