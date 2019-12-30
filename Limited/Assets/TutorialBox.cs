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
	public void Enable(bool value)
	{
		gameObject.SetActive(value);
	}

	public void OnClick()
	{
		// trigger closing animation
		animator.SetTrigger("CloseTrigger");
	}

	public void OnExitAnimationEnd()
	{
        Debug.Log("Animation End");
		// when the animation is finished, it will call the NextMessage function of TutorialController
		controller.NextBox();
	}
}
