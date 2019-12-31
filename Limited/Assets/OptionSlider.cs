using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class OptionSlider : MonoBehaviour
{
	public string optionName;
	public Slider slider;
	public TextMeshProUGUI percentage;
	public AudioMixer mixer;

	void Start()
	{
		if (!PlayerPrefs.HasKey(optionName)) return;

		// initialize slider at the correct value
		float value = PlayerPrefs.GetFloat(optionName);

		slider.value = value;
		percentage.text = Mathf.RoundToInt(value * 100) + "%";

	}

	public void OnValueChanged()
	{
		/* function called when there is a change on the slider associated.
		   triggered by the slider element
		 */

		PlayerPrefs.SetFloat(optionName, slider.value);

		mixer.SetFloat(optionName, Mathf.Log10(slider.value) * 20);

		percentage.text = Mathf.RoundToInt(slider.value * 100) + "%";
	}
}
