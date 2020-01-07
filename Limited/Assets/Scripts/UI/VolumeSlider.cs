using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
	public string optionName;
	public Slider slider;
	public TextMeshProUGUI percentage;
	public AudioMixer mixer;

	void Start()
	{
		// stop if there is no key corresponding to this slider
		if (!PlayerPrefs.HasKey(optionName)) return;

		// initialize slider and text display at the correct value
		float value = PlayerPrefs.GetFloat(optionName);
		slider.value = value;
		percentage.text = Mathf.RoundToInt(value * 100) + "%";

	}

	public void OnValueChanged()
	{
		/* function called when there is a change on the slider associated.
		   triggered by the slider element
		 */

		// save value
		PlayerPrefs.SetFloat(optionName, slider.value);

		// change volume
		mixer.SetFloat(optionName, Mathf.Log10(slider.value) * 20);

		// change percentage text
		percentage.text = Mathf.RoundToInt(slider.value * 100) + "%";
	}
}
