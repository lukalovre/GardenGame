using Assets.Code;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private void Start()
	{
		if(GlobalSettings.MusicOn)
		{
			GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayMusic();
		}
		else
		{
			GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().StopMusic();
		}
	}
}