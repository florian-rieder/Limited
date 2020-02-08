using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialBox : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textBox;
	[SerializeField] private TutorialController controller;
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject dots;

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

	public void TriggerClosingAnimation()
	{
		animator.SetTrigger("CloseTrigger");
	}

	public void ShowDots(bool value)
	{
		dots.SetActive(value);
	}
}
