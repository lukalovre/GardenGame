using Assets.Code;
using Assets.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	public static List<GameObject> GameObjectList;
	public static bool[,] Grid;
	public GameObject Leaf;
	public GameObject Player;
	public GameObject Rock;
	public GameObject Snail;
	public GameObject Snail2;
	public GameObject Snail3;
	public GameObject Strawberry;
	public GameObject TilemapTerrain;

	private bool m_levelOver;

	private void AddAndRemoveRocks(int width, int heigth)
	{
		for(int y = 0; y < heigth; y++)
		{
			for(int x = 0; x < width; x++)
			{
				if(Grid[x, y])
				{
					if(Random.Range(0, GlobalSettings.RockAmount + 1) == 0)
					{
						Grid[x, y] = false;

						continue;
					}

					var rock = GameObjectPool.Create(Rock);
					rock.transform.position = new Vector3(x, y);
				}
			}
		}
	}

	private void CheckIfLevelOver()
	{
		var lose = Strawberry.GetComponent<Snack>().IsDead() && GameObject.FindGameObjectsWithTag("AI").All(ai => ai.GetComponent<AI>().DoneTurn);

		if(lose)
		{
			StartCoroutine(WaitForLoseSound());
		}
	}

	private void GenerateGrid(int width, int height)
	{
		m_levelOver = false;

		GameObject.FindGameObjectsWithTag(Rock.tag).ToList().ForEach(GameObjectPool.Delete);
		GameObject.FindGameObjectsWithTag(Leaf.tag).ToList().ForEach(GameObjectPool.Delete);
		GameObject.FindGameObjectsWithTag(GameObject.Find("Explosion").tag).ToList().ForEach(GameObjectPool.Delete);

		TilemapTerrain.GetComponent<Terrain>().GenerateGrid(width, height);

		GameObjectList = new List<GameObject>
		{
			Strawberry,
			Player,
			Snail,
			Snail2,
			Snail3
		};

		Grid = Maze.GenerateMaze(width, height);

		SetGameObjectPositions(width, height);

		foreach(var gameObject in GameObjectList)
		{
			gameObject.GetComponent<ILoad>()?.Load();
		}

		SwitchSomeRocksWithLeaves(width, height);
	}

	private void SetGameObjectPositions(int width, int heigth)
	{
		AddAndRemoveRocks(width, heigth);

		var bottomHalf = heigth / 2;
		var safeDistance = 1;

		var emptyTiles = Grid.GetEmptyTiles();

		var bottomTiles = emptyTiles.Where(tile => tile.y < bottomHalf - safeDistance);
		var bottomTilesShuffled = new Stack<Vector3>(Helper.Shuffle(bottomTiles.ToList()));

		var topTiles = emptyTiles.Where(tile => tile.y >= bottomHalf + safeDistance);
		var topTilesShuffled = new Stack<Vector3>(Helper.Shuffle(topTiles.ToList()));

		foreach(var gameObject in GameObjectList.Where(o => !o.CompareTag(Leaf.tag)))
		{
			Vector3 position;

			if(gameObject.CompareTag(Snail.tag))
			{
				position = topTilesShuffled.Pop();
			}
			else
			{
				position = bottomTilesShuffled.Pop();
			}

			gameObject.transform.position = position;
		}
	}

	private void Start()
	{
		GenerateGrid(GlobalSettings.Width, GlobalSettings.Height);
	}

	private void SwitchSomeRocksWithLeaves(int width, int heigth)
	{
		for(int y = 0; y < heigth; y++)
		{
			for(int x = 0; x < width; x++)
			{
				if(!Grid[x, y])
				{
					continue;
				}

				if(Random.Range(0, 5 - GlobalSettings.LeafAmount) == 0)
				{
					var rock = GameObject.FindGameObjectsWithTag(Rock.tag).FirstOrDefault(o => o.transform.position == new Vector3(x, y));

					if(rock != null)
					{
						GameObjectPool.Delete(rock);
					}

					var leaf = GameObjectPool.Create(Leaf);
					leaf.transform.position = new Vector3(x, y);
					leaf.GetComponent<Snack>().Load();
					GameObjectList.Add(leaf);
					Grid[x, y] = false;
				}
			}
		}
	}

	private void Update()
	{
		CheckIfLevelOver();

		if(m_levelOver)
		{
			GenerateGrid(GlobalSettings.Width, GlobalSettings.Height);
		}

		if(Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			int width = Random.Range(GlobalSettings.MIN_WIDTH, GlobalSettings.MAX_WIDTH);
			int heigth = Random.Range(GlobalSettings.MIN_HEIGHT, GlobalSettings.MAX_HEIGHT);

			GenerateGrid(width, heigth);
		}
	}

	private IEnumerator WaitForLoseSound()
	{
		if(GlobalSettings.SoundOn)
		{
			GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().StopMusic();

			var source = GameObject.Find("Lose").GetComponent<AudioSource>();
			source.Play();

			while(source.isPlaying)
			{
				yield return null;
			}

			GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayMusic();
		}

		m_levelOver = true;
	}
}