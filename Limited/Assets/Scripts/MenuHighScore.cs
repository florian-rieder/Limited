using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHighScore : MonoBehaviour
{
	private TextMeshProUGUI _text;
	// Start is called before the first frame update
	void Start()
	{
		_text = GetComponent<TextMeshProUGUI>();
		_text.text = "High score: " + PlayerPrefs.GetInt("Highscore").ToString();
	}
}
