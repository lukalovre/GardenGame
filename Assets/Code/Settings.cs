using Assets.Code;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
	public string NextScene;
	public bool Series;
	public bool ToggleButton;
	public int Value;
	public int Height { get; private set; } = GlobalSettings.MAX_HEIGHT;
	public bool ToggleValue { get; private set; }
	public int Width { get; private set; } = GlobalSettings.MAX_WIDTH;

	private void ButtonClick()
	{
		if(ToggleButton)
		{
			ToggleValue = !ToggleValue;
			GetComponent<Animator>().enabled = ToggleValue;
			GetComponent<SpriteRenderer>().color = ToggleValue ? Color.white : Color.black;
			return;
		}

		LoadNextScene();
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene(NextScene);
	}

	private void OnMouseDown()
	{
		ButtonClick();
	}
}