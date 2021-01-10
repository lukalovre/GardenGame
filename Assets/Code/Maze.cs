using UnityEngine;

namespace Assets.Code
{
	public static class Maze
	{
		public static bool[,] grid;
		private static int s_height;
		private static int s_width;

		public static bool[,] GenerateMaze(int width, int height)
		{
			s_width = width;
			s_height = height;

			grid = new bool[s_width, s_height];

			//Initialize Grid with all walls
			for(int x = 0; x < s_width; x++)
			{
				for(int y = 0; y < s_height; y++)
				{
					grid[x, y] = true;
				}
			}

			int startingRow = 0;
			int startingColumn = 0;

			MakeMazePaths(startingRow, startingColumn);

			return grid;
		}

		//Checks to see if any of the surrounding cells are unvisited
		private static bool HasUnvisited(int x, int y)
		{
			// Up
			if(y + 2 > s_height - 1)
			{
			}
			else if(grid[x, y + 2])
			{
				return true;
			}

			// Down
			if(y - 2 < 0)
			{
			}
			else if(grid[x, y - 2])
			{
				return true;
			}

			// Left
			if(x - 2 < 0)
			{
			}
			else if(grid[x - 2, y])
			{
				return true;
			}

			// Right
			if(x + 2 > s_width - 1)
			{
			}
			else if(grid[x + 2, y])
			{
				return true;
			}

			return false;
		}

		private static void MakeMazePaths(int x, int y)
		{
			// Remove wall from current cell
			grid[x, y] = false;

			if(!HasUnvisited(x, y))
			{
				return;
			}

			switch(Random.Range(0, 4))
			{
				// Up
				case 0:

					if(y + 2 > s_height - 1)
					{
					}
					else if(grid[x, y + 2])
					{
						grid[x, y + 1] = false;
						MakeMazePaths(x, y + 2);
					}

					break;

				// Down
				case 1:

					if(y - 2 < 0)
					{
					}
					else if(grid[x, y - 2])
					{
						grid[x, y - 1] = false;
						MakeMazePaths(x, y - 2);
					}

					break;

				// Left
				case 2:

					if(x - 2 < 0)
					{
					}
					else if(grid[x - 2, y])
					{
						grid[x - 1, y] = false;
						MakeMazePaths(x - 2, y);
					}

					break;

				// Right
				case 3:

					if(x + 2 > s_width - 1)
					{
					}
					else if(grid[x + 2, y])
					{
						grid[x + 1, y] = false;
						MakeMazePaths(x + 2, y);
					}

					break;
			}

			MakeMazePaths(x, y);
		}
	}
}