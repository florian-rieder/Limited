using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;
	public AudioMixer mixer;

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

			// route the output of this source to the "Effects" group in the game's audio mixer
			sound.source.outputAudioMixerGroup = mixer.FindMatchingGroups("SoundEffects")[0];
		}

		// play the game music and stop the menu music
		if (GameObject.FindGameObjectWithTag("MenuMusic") == null) return;
		GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<MenuMusic>().PlayGame();
	}

	public void Play(string soundName)
	{
		Sound s = Array.Find(sounds, sound => sound.name == soundName);
		if (s != null) s.source.Play();
	}
}
