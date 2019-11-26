using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopBarResourceDisplay : MonoBehaviour
{
	private string Name;

	[SerializeField]
	private TextMeshProUGUI text;
	[SerializeField]
	private SpriteRenderer iconRenderer;

	public void SetValue(int value)
	{
		Color color = GameSystem.SignColor(value);

		text.color = color;
		text.text = value.ToString();
	}

	public void SetIcon(Sprite sprite)
	{
		iconRenderer.sprite = sprite;
	}

	public void SetName(string name)
	{
		Name = name;
	}

	public string GetName()
	{
		return Name;
	}
}
