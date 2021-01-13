using Assets.Code;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
	public string NextScene;
	public bool Toogle;
	private Collider2D m_collider;
	private bool m_toogleValue;

	private void ButtonClick()
	{
		if(Toogle)
		{
			m_toogleValue = !m_toogleValue;
			GetComponent<Animator>().enabled = m_toogleValue;
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