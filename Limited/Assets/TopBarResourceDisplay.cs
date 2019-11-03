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
	[SerializeField]
	private Color32 negativeColor;
	[SerializeField]
	private Color32 positiveColor;

	public void SetValue(int value)
	{
		/* if(value > 0){
			text.color = negativeColor;
		} else {
			text.color = positiveColor;
		} */

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
