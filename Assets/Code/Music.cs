using UnityEngine;

public class Music : MonoBehaviour
{
	private AudioSource m_audioSource;

	public void PlayMusic()
	{
		if(m_audioSource.isPlaying)
		{
			return;
		}

		m_audioSource.Play();
	}

	public void StopMusic()
	{
		m_audioSource.Stop();
	}

	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
		m_audioSource = GetComponent<AudioSource>();
	}
}