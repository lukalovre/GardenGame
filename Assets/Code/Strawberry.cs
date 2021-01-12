using Assets.Code;
using UnityEngine;

public class Strawberry : MonoBehaviour, ILoad
{
	public Sprite FullHealth;
	public Sprite HalfHealth;
	public Sprite LastHealth;
	private int m_health;

	public void Load()
	{
		m_health = 3;
	}

	internal void Bite()
	{
		m_health--;
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
}