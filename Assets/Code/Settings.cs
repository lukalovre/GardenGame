using Assets.Code;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
	public string NextScene;
	public bool Series;
	public bool ToggleButton;
	public int Value;

	public bool ToggleValue { get; private set; }

	private void ButtonClick()
	{
		if(Series)
		{
			SelectValue(Value);
			return;
		}

		if(ToggleButton)
		{
			HandleToggleButton();
			return;
		}

		LoadNextScene();
	}

	private void ChangeButtonStatus(bool freeze)
	{
		var animator = GetComponent<Animator>();

		if(animator != null)
		{
			animator.enabled = !freeze;
		}

		GetComponent<SpriteRenderer>().color = freeze ? Color.black : Color.white;
	}

	private void HandleToggleButton()
	{
		ToggleValue = !ToggleValue;
		ChangeButtonStatus(!ToggleValue);

		if(gameObject.name == "Music")
		{
			GlobalSettings.MusicOn = ToggleValue;

			if(GlobalSettings.MusicOn)
			{
				GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayMusic();
			}
			else
			{
				GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().StopMusic();
			}
		}
		else
		{
			GlobalSettings.SoundOn = ToggleValue;
		}
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene(NextScene);
	}

	private void OnMouseDown()
	{
		ButtonClick();
	}

	private void SelectValue(int value)
	{
		if(gameObject.CompareTag("Width"))
		{
			GlobalSettings.Width = value;
		}

		if(gameObject.CompareTag("Height"))
		{
			GlobalSettings.Height = value;
		}

		if(gameObject.CompareTag("Rocks"))
		{
			GlobalSettings.RockAmount = value;
		}

		if(gameObject.CompareTag("Leaves"))
		{
			GlobalSettings.LeafAmount = value;
		}

		foreach(var gameObject in GameObject.FindGameObjectsWithTag(gameObject.tag))
		{
			var gameObjectValue = gameObject.GetComponent<Settings>().Value;

			gameObject.GetComponent<Settings>().ChangeButtonStatus(gameObjectValue != value);
		}
	}

	private void Start()
	{
		if(Series)
		{
			if(gameObject.CompareTag("Width"))
			{
				SelectValue(GlobalSettings.Width);
			}

			if(gameObject.CompareTag("Height"))
			{
				SelectValue(GlobalSettings.Height);
			}

			if(gameObject.CompareTag("Rocks"))
			{
				SelectValue(GlobalSettings.RockAmount);
			}

			if(gameObject.CompareTag("Leaves"))
			{
				SelectValue(GlobalSettings.LeafAmount);
			}

			return;
		}

		if(ToggleButton)
		{
			if(gameObject.name == "Music")
			{
				ToggleValue = GlobalSettings.MusicOn;
			}
			else
			{
				ToggleValue = GlobalSettings.SoundOn;
			}

			ChangeButtonStatus(!ToggleValue);
		}
	}
}