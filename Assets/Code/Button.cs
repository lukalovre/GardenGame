using Assets.Code;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
	public string NextScene;
	private Collider2D m_collider;

	private void LoadNextScene()
	{
		SceneManager.LoadScene(NextScene);
	}

	private void OnMouseDown()
	{
		LoadNextScene();
	}

	private void Start()
	{
		m_collider = GetComponent<Collider2D>();
	}

	private void Update()
	{
		if(TouchInput.IsTouched(m_collider))
		{
			LoadNextScene();
		}
	}
}