using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;

	// Use this for initialization
	void Awake()
	{
		// create an audio source for each sound we set in the inspector
		foreach (Sound sound in sounds)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
		}
	}

	public void Play(string soundName)
	{
		Sound s = Array.Find(sounds, sound => sound.name == soundName);
		if (s != null) s.source.Play();
	}
}
