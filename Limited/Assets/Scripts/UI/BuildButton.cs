using UnityEngine;
using TMPro; // Text Mesh Pro namespace
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_text; // the text component of our button
	[SerializeField]
	private ButtonListControl btnControl;
	public Image m_icon;
	public GameObject resourceDisplayTemplate;
	public Texture2D resourcesSpritesheet;
	private string m_textString;
	private FacilitiesTileType m_type;

	public void SetText(string textString)
	{
		// we store the text string and assign it to our text component
		m_textString = textString;
		m_text.text = m_textString;
	}
	public string GetText()
	{
		return m_textString;
	}
	public void SetType(FacilitiesTileType type)
	{
		m_type = type;
	}
	public void SetImage(Sprite sprite){
		m_icon.sprite = sprite;
	}

	public FacilitiesTileType GetTileType()
	{
		return m_type;
	}

	public void OnClick()
	{
		btnControl.ButtonClicked(m_type);
	}

	public void GenerateResourcesDisplay()
	{
		var resources = m_type.GenerateResourcesDictionary();
		List<string> emptyResourceIndexes = new List<string>();

		// remove entries with a value of 0
		foreach (KeyValuePair<string, int> entry in resources)
		{
			if (entry.Value == 0) emptyResourceIndexes.Add(entry.Key);
		}
		foreach (string key in emptyResourceIndexes)
		{
			resources.Remove(key);
		}

		// generate displays
		foreach (KeyValuePair<string, int> entry in resources)
		{
			string name = entry.Key;
			int value = entry.Value;

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

			// get sprite
			var names = new Dictionary<string, int>{
				{"Oil", 0},
				{"Coal", 1},
				{"Wood", 2},
				{"Metal", 6},
				{"Power", 3},
				{"Goods", 4},
				{"Food", 5}
			};
			
			// get all sliced tiles from our tileset
			Sprite[] resourceSprites = Resources.LoadAll<Sprite>(resourcesSpritesheet.name);
			Sprite sprite = resourceSprites[names[name]];

			// apply sprite and value to item
			GameObject display = Instantiate(resourceDisplayTemplate);
			BuildButtonResourceDisplay displayScript = display.GetComponent<BuildButtonResourceDisplay>();
			display.SetActive(true);
			displayScript.SetImage(sprite);
			displayScript.SetText(valueString);

			// place display in the hierarachy
			display.transform.SetParent(resourceDisplayTemplate.transform.parent);
		}
	}
}
