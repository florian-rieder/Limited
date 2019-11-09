using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FamineTimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI display;
    // Update is called once per frame
    void Update()
    {
        display.text = "Low supplies: " + Mathf.RoundToInt(GameController.instance.GetFamineTimeRemaining()) + " seconds left";
    }

    public void Enable(bool value){
        gameObject.SetActive(value);
    }
}
