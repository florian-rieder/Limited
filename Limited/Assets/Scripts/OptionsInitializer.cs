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
        if (PlayerPrefs.HasKey("MasterVolume")) mixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
	}
}
