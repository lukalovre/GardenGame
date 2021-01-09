using UnityEngine;

public class AI : MonoBehaviour
{
	private Collider2D collider;

	private void OnTriggerEnter2D(Collider2D collision)
	{
	}

	// Start is called before the first frame update
	private void Start()
	{
		collider = GetComponent<Collider2D>();
	}

	// Update is called once per frame
	private void Update()
	{
		if(Input.touchCount == 0)
		{
			return;
		}

		var touch = Input.GetTouch(0);
		var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
		var touchedCollider = Physics2D.OverlapPoint(touchPosition);

		if(collider == touchedCollider)
		{
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 0), 1 * Time.deltaTime);
		}
	}
}