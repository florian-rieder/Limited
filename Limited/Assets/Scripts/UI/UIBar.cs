using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar : MonoBehaviour
{
	public GameObject bar;
	private RectTransform tf;
	private float currentValue = 0f;

	// Start is called before the first frame update
	void Awake()
	{
		tf = (RectTransform)bar.transform;
		tf.localScale = new Vector3(0, 1, 1);
	}

	public void SetValue(float value)
	{
		if (value <= 1 && value >= 0 && value != currentValue)
		{
			tf.localScale = new Vector3(value, 1, 1);
			currentValue = value;
		}
	}

	public void Enable(bool value)
	{
		gameObject.SetActive(value);
	}
}
