using UnityEngine;

namespace Assets.Code
{
	public static class Maze
	{
		public static bool[,] grid;
		private static int s_height;
		private static int s_width;

		public static bool[,] GenerateMaze(int width, int height, Vector2Int startPosition = default)
		{
			s_width = width;
			s_height = height;

			grid = new bool[s_width, s_height];

			//Initialize grid with all walls, true == wall
			for(int x = 0; x < s_width; x++)
			{
				for(int y = 0; y < s_height; y++)
				{
					grid[x, y] = true;
				}
			}

			MakeMazePaths(startPosition.x, startPosition.y);

			return grid;
		}

		//Checks to see if any of the surrounding cells are unvisited
		private static bool HasUnvisited(int x, int y)
		{
			return UnvisitedUp(x, y) || UnvisitedDown(x, y) || UnvisitedLeft(x, y) || UnvisitedRight(x, y);
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

					if(UnvisitedUp(x, y))
					{
						grid[x, y + 1] = false;
						MakeMazePaths(x, y + 2);
					}
					//else if(y + 1 < s_height)
					//{
					//	grid[x, y + 1] = false;
					//}

					break;

				case 1:

					if(UnvisitedDown(x, y))
					{
						grid[x, y - 1] = false;
						MakeMazePaths(x, y - 2);
					}

					break;

				case 2:

					if(UnvisitedLeft(x, y))
					{
						grid[x - 1, y] = false;
						MakeMazePaths(x - 2, y);
					}

					break;

				case 3:

					if(UnvisitedRight(x, y))
					{
						grid[x + 1, y] = false;
						MakeMazePaths(x + 2, y);
					}

					break;
			}

			MakeMazePaths(x, y);
		}

		private static bool UnvisitedDown(int x, int y)
		{
			return y - 2 >= 0 && grid[x, y - 2];
		}

		private static bool UnvisitedLeft(int x, int y)
		{
			return x - 2 >= 0 && grid[x - 2, y];
		}

		private static bool UnvisitedRight(int x, int y)
		{
			return x + 2 < s_width && grid[x + 2, y];
		}

		private static bool UnvisitedUp(int x, int y)
		{
			return y + 2 < s_height && grid[x, y + 2];
		}
	}
}