using Assets.Code;
using UnityEngine;

public class Strawberry : MonoBehaviour, ILoad
{
	public Sprite FullHealth;
	public Sprite HalfHealth;
	public Sprite LastHealth;
	private const int FULL_HEALTH = 3;
	private int m_health;

	public void Load()
	{
		m_health = FULL_HEALTH;
		UpdateSprite();
	}

	internal void Bite()
	{
		m_health--;
		UpdateSprite();
	}

	internal bool IsDead()
	{
		return m_health <= 0;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void UpdateSprite()
	{
		Sprite sprite;

		if(m_health == FULL_HEALTH)
		{
			sprite = FullHealth;
		}
		else if(m_health == 1)
		{
			sprite = LastHealth;
		}
		else if(m_health <= 0)
		{
			sprite = null;
		}
		else
		{
			sprite = HalfHealth;
		}

		GetComponent<SpriteRenderer>().sprite = sprite;
	}
}