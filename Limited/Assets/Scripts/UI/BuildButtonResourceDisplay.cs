using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildButtonResourceDisplay : MonoBehaviour
{
	public Image m_image;
	public TextMeshProUGUI m_text;

	void Start()
	{
		RectTransform transfo = (RectTransform)transform;
		transfo.localScale = new Vector3(2, 2, 2);
	}

	public void SetValue(int value)
	{
		Color color = GameSystem.SignColor(value);
		string text = formatValue(value);

		m_text.color = color;
		m_text.text = text;
	}

	public void SetImage(Sprite sprite)
	{
		m_image.sprite = sprite;
	}

	public void Enable(bool value)
	{
		gameObject.SetActive(value);
	}

	private string formatValue(int value)
	{
		// format value
		string valueString = "";
		if (value > 0)
		{
			valueString = "+" + value;
		}
		else
		{
			valueString = value.ToString();
		}
		return valueString;
	}
}
