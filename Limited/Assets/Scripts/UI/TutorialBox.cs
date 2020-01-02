using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialBox : MonoBehaviour
{
	public TextMeshProUGUI textBox;
	public TutorialController controller;
	public Animator animator;

	public void SetText(string text)
	{
		// set message of the box
		textBox.text = text;
	}

	public void OnClick()
	{
		controller.NextBox();
	}

	public void OnExitAnimationEnd()
	{
		// when the animation is finished, disable this object
		gameObject.SetActive(false);
	}
}
