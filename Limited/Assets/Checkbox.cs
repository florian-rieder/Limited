using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkbox : MonoBehaviour
{
	public string optionName;

	[Range(0f, 1f)]
	public int value; // int because PlayerPrefs can't handle booleans

	void Start()
	{
        // if the preference setting this checkbox uses exists
		if (PlayerPrefs.HasKey(optionName))
		{
			// update graphics according to saved preference
			gameObject.GetComponent<Image>().color = PlayerPrefs.GetInt(optionName) == 1 ? Color.white : Color.black;
		}
		else
		{
            // create new preference setting with default value
			PlayerPrefs.SetInt(optionName, value);
		}
	}

	public void OnClick()
	{
		/* called on click event provided by the Event Trigger component */

		// toggle between true (1) and false (0)
		int newValue = PlayerPrefs.GetInt(optionName) == 1 ? 0 : 1;

		// save new value
		PlayerPrefs.SetInt(optionName, newValue);

		// update graphics
		gameObject.GetComponent<Image>().color = newValue == 1 ? Color.white : Color.black;

	}
}
