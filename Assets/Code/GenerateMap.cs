using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	private void GenerateGrid(int rows, int columns)
	{
		var tile = Instantiate(Resources.Load("GrassTile"));

		for(int y = 0; y < columns; y++)
		{
			for(int x = 0; x < rows; x++)
			{
			}
		}
	}

	// Start is called before the first frame update
	private void Start()
	{
		GenerateGrid(10, 5);
	}

	// Update is called once per frame
	private void Update()
	{
	}
}