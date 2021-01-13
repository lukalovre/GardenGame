using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
	private Collider2D m_collider;

	private bool IsTouched()
	{
		if(Input.touchCount == 0)
		{
			return false;
		}

		var touch = Input.GetTouch(0);
		var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
		var touchedCollider = Physics2D.OverlapPoint(touchPosition);
		return m_collider == touchedCollider && touch.phase == TouchPhase.Began;
	}

	private void OnMouseDown()
	{
		PlayGame();
	}

	private void PlayGame()
	{
		SceneManager.LoadScene("Main");
	}

	private void Start()
	{
		m_collider = GetComponent<Collider2D>();
	}

	private void Update()
	{
		if(IsTouched())
		{
			PlayGame();
		}
	}
}