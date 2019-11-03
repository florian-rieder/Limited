using UnityEngine;
using TMPro; // Text Mesh Pro namespace
using System.Collections.Generic;

public class BuildButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_text; // the text component of our button
	[SerializeField]
	private ButtonListControl btnControl;
	[SerializeField]
	private PlayerInventory playerInventory;

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

	public void OnClick()
	{
		btnControl.ButtonClicked(m_type);
	}

	public bool IsBuildable(EnvironmentTile tile)
	{
		Dictionary<string, int> inventory = playerInventory.getCount();
		Dictionary<string, int> typeResourcesDictionary = m_type.GenerateResourcesDictionary();

		bool canBuild = true;

		// check if the player has the resources that are needed to build this facility
		foreach (KeyValuePair<string, int> resource in inventory)
		{
			string name = resource.Key;
			int availableAmount = resource.Value;

			int typeAmountForThisRessource = typeResourcesDictionary[name];

			if (m_type.Extractor && GameTiles.instance.EnvironmentResourceNames.Contains(name))
			{
				// get the amount of this ressource in the tile we want to build on
				int tileAmountOfThisRessource = tile.Resources[name];

				// if the amount of this resource in the ground is insufficient
				if (tileAmountOfThisRessource < typeAmountForThisRessource)
				{
					canBuild = false;
					break;
				}
			}
			// if the resource is extracted from the tile by the facility and the resource can be extracted from the ground
			// for example, a coal mine needs power, and coal in the ground, but power can't be extracted from the ground, but
			// taken from the player's inventory.
			// TL;DR - we only apply this "ground" check for the resources that can be in the ground.

			// if the resource is taken from the inventory by the facility
			// only for the numbers indicating a consumption (negative numbers)
			else if (typeAmountForThisRessource < 0)
			{
				// if the amount in the inventory of the player is insufficient
				if (availableAmount < Mathf.Abs(typeAmountForThisRessource))
				{
					canBuild = false;
					break;
				}
			}
		}

		if (canBuild)
		{
			// custom rules
			if (m_type.Name == "Farm" && tile.Polluted == true) canBuild = false;
		}

		return canBuild;
	}
}
