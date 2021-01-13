using UnityEngine;

namespace Assets.Code
{
	public static class TouchInput
	{
		public static bool IsTouched(Collider2D collider)
		{
			if(Input.touchCount == 0)
			{
				return false;
			}

			var touch = Input.GetTouch(0);
			var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
			var touchedCollider = Physics2D.OverlapPoint(touchPosition);
			return collider == touchedCollider && touch.phase == TouchPhase.Began;
		}
	}
}