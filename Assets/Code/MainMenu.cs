using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private void Start()
	{
		GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayMusic();
	}
}