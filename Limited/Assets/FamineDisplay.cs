using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamineDisplay : MonoBehaviour
{
	public UIBar bar;

	void Update()
	{
		bar.SetValue(GameController.instance.GetFamineTimeRemaining() / GameController.instance.famineTimerBase);
	}

	public void Enable(bool value)
	{
		gameObject.SetActive(value);
	}
}
