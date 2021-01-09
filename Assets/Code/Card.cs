using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
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
		var aiList = GameObject.FindGameObjectsWithTag("AI");

		if(aiList.ToList().Any(ai => ai.GetComponent<AI>().DoneTurn))
		{
			AI.DoTurn = false;
		}

		if(Input.touchCount == 0)
		{
			return;
		}

		var touch = Input.GetTouch(0);
		var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
		var touchedCollider = Physics2D.OverlapPoint(touchPosition);

		if(collider == touchedCollider && touch.phase == TouchPhase.Began)
		{
			AI.DoTurn = true;
		}
	}
}