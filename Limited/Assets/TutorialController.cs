using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
	public TutorialBox tutorialBox;

	[SerializeField]
	private string[] messages;
	private int currentMessageIndex = 0;

	public void Initialize()
	{
		if (!PlayerPrefs.HasKey("TutorialEnabled"))
		{
			// if the key has not been initialized, initialize it to true
			// (Enable tutorial by default since this is the first launch of the game)
			PlayerPrefs.SetInt("TutorialEnabled", 1);
			return;
		}

		// tutorial is enabled
		if (PlayerPrefs.GetInt("TutorialEnabled") == 1)
		{
            tutorialBox.gameObject.SetActive(true);
			NextBox();
		}
		// tutorial is disabled
		else
		{
			// disable this script
			gameObject.SetActive(false);
		}
	}

	public void NextBox()
	{
        Debug.Log("NextBox 1");
		if (currentMessageIndex > messages.Length) return;
        Debug.Log("NextBox 2");

		// display current message
		tutorialBox.SetText(messages[currentMessageIndex]);
		// show the box
        Debug.Log("OpenTrigger");
		tutorialBox.animator.SetTrigger("OpenTrigger");
		// set next message index
		currentMessageIndex++;
	}
}
