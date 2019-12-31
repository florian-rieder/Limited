using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionSlider : MonoBehaviour
{
	public string optionName;
	public Slider slider;
	public TextMeshProUGUI percentage;
	public AudioMixer mixer;

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
