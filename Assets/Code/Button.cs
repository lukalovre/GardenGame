using Assets.Code;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
	public string NextScene;
	public bool ToggleButton;
	public bool ToggleValue = true;
	private Collider2D m_collider;

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

	private void Start()
	{
		m_collider = GetComponent<Collider2D>();
	}

	private void Update()
	{
		if(TouchInput.IsTouched(m_collider))
		{
			ButtonClick();
		}
	}
}