using Assets.Code;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
	public string NextScene;
	public bool Series;
	public bool ToggleButton;
	public int Value;
	private Collider2D m_collider;
	private bool m_mouseClicked;
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
		m_mouseClicked = true;
	}

	private void SelectValue(int value)
	{
		if(CompareTag("Width"))
		{
			GlobalSettings.Width = value;
		}

		if(CompareTag("Height"))
		{
			GlobalSettings.Height = value;
		}

		if(CompareTag("Rocks"))
		{
			GlobalSettings.RockAmount = value;
		}

		if(CompareTag("Leaves"))
		{
			GlobalSettings.LeafAmount = value;
		}

		foreach(var gameObject in GameObject.FindGameObjectsWithTag(tag))
		{
			var gameObjectValue = gameObject.GetComponent<Settings>().Value;

			gameObject.GetComponent<Settings>().ChangeButtonStatus(gameObjectValue != value);
		}
	}

	private void Start()
	{
		m_collider = GetComponent<Collider2D>();

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

	private void Update()
	{
		if(TouchInput.IsTouched(m_collider) || m_mouseClicked)
		{
			m_mouseClicked = false;

			ButtonClick();
		}
	}
}