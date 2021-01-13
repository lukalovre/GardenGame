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
		GetComponent<Animator>().enabled = !freeze;
		GetComponent<SpriteRenderer>().color = freeze ? Color.black : Color.white;
	}

	private void HandleToggleButton()
	{
		if(gameObject.name == "Music")
		{
			GlobalSettings.MusicOn = ToggleValue;
		}
		else
		{
			GlobalSettings.SoundOn = ToggleValue;
		}

		ToggleValue = !ToggleValue;
		ChangeButtonStatus(ToggleValue);
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
		else
		{
			GlobalSettings.Height = value;
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
				SelectValue(GlobalSettings.MAX_WIDTH);
			}
			else
			{
				SelectValue(GlobalSettings.MAX_HEIGHT);
			}

			return;
		}
	}
}