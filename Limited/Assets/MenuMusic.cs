using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
	private AudioSource _audioSource;
	private MenuMusic instance;
	private void Awake()
	{
		/* // Singleton
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
            return;
		} */

		int numMusicPlayers = FindObjectsOfType<MenuMusic>().Length;
		if (numMusicPlayers != 1)
		{
			Destroy(this.gameObject);
		}
		// if more then one music player is in the scene
		//destroy ourselves
		else
		{
			DontDestroyOnLoad(gameObject);
		}

		_audioSource = GetComponent<AudioSource>();
	}

	public void PlayMusic()
	{
		if (_audioSource.isPlaying) return;
		_audioSource.Play();
	}

	public void StopMusic()
	{
		_audioSource.Stop();
	}
}
