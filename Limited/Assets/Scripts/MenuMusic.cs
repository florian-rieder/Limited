using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
	[SerializeField]
	private AudioSource menuSource;
	[SerializeField]
	private AudioSource gameSource;
	private MenuMusic instance;
	private void Awake()
	{
		// prevent gameobject from duplicating because of the DontDestroyOnLoad
		int numMusicPlayers = FindObjectsOfType<MenuMusic>().Length;
		if (numMusicPlayers != 1)
		{
			Destroy(this.gameObject);
		}
		// if more then one music player is in the scene
		// destroy ourselves
		else
		{
			DontDestroyOnLoad(gameObject);
		}
	}

	public void PlayMenu()
	{
		if (gameSource.isPlaying) gameSource.Stop();
		if (!menuSource.isPlaying) menuSource.Play();
	}

	public void PlayGame()
	{
		if (menuSource.isPlaying) menuSource.Stop();
		if (!gameSource.isPlaying) gameSource.Play();
	}
}
