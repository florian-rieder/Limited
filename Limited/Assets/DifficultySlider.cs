using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultySlider : MonoBehaviour
{
	public Slider slider;
	public TextMeshProUGUI display;

	public DifficultyLevel[] difficultyLevels;

	void Start()
	{
		if (!PlayerPrefs.HasKey("difficulty"))
		{
			PlayerPrefs.SetFloat("difficulty", 0.5f);
		}

		// initialize slider and text display at the correct value
		float value = PlayerPrefs.GetFloat("difficulty");
		slider.value = value;

		DisplayDifficulty(value);

	}

	public void OnValueChanged()
	{
		/* function called when there is a change on the slider associated.
		   triggered by the slider element
		 */

		// save value
		PlayerPrefs.SetFloat("difficulty", slider.value);

		DisplayDifficulty(slider.value);
	}

	private void DisplayDifficulty(float level)
	{
		/* needs the difficulty levels list to be ordered from easy to hard */

		foreach (var difficultyLevel in difficultyLevels)
		{
			if (difficultyLevel.threshold > level) continue;

			display.text = difficultyLevel.Name;
			display.color = difficultyLevel.color;
		}
	}
}

[System.Serializable]
public class DifficultyLevel
{
	public string Name;
	public Color color;
	public float threshold;
}
