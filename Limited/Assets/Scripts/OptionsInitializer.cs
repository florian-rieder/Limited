using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsInitializer : MonoBehaviour
{
	public AudioMixer mixer;

	// Start is called before the first frame update
	void Start()
	{
		if (!PlayerPrefs.HasKey("MasterVolume")) PlayerPrefs.SetFloat("MasterVolume", 1f);
		mixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);

		if (!PlayerPrefs.HasKey("SoundEffectsVolume")) PlayerPrefs.SetFloat("SoundEffectsVolume", 1f);
		mixer.SetFloat("SoundEffectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("SoundEffectsVolume")) * 20);

		if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 1f);
		mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);

		if (!PlayerPrefs.HasKey("TutorialEnabled")) PlayerPrefs.SetInt("TutorialEnabled", 1);

		if (!PlayerPrefs.HasKey("difficulty")) PlayerPrefs.SetFloat("Difficulty", 0.5f);
		if (!PlayerPrefs.HasKey("Highscore")) PlayerPrefs.SetInt("Highscore", 0);

	}
}
