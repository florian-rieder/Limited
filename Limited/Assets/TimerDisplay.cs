using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
        int timeRemaining = Mathf.RoundToInt(GameController.instance.GetTimeRemaining());
		TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
		text.text = "City grows in " + timeRemaining + " seconds";
	}

    public void Enable(bool value){
        gameObject.SetActive(value);
    }
}
