using UnityEngine;

public class ButtonListControl : MonoBehaviour
{
	[SerializeField]
	private GameObject buttonTemplate;

	//private FacilitiesTileTypeRoot facilitiesTypes;


	public void ButtonClicked(string textString)
	{
		Debug.Log(textString);
	}

	public void UpdateButtons(EnvironmentTile tile)
	{

		var facilitiesTypes = GameTiles.instance.GetFacilitiesTypes();
		// empty parent object

		// iterate through all children of our content object and destroy them
		foreach (Transform child in buttonTemplate.transform.parent)
		{
			// don't destroy button template
			if (child.name == buttonTemplate.transform.name) continue;

			GameObject.Destroy(child.gameObject);
		}

		// repopulate our dialog box

		// if there is no tile there is nothing that can be built
		if (tile != null)
		{
			// can't build anything on water
			if (tile.Name == "Water") return;

			// maybe sort types in some manner

			// create a button for each possible facility that can be built
			foreach (FacilitiesTileType type in facilitiesTypes.tileTypes)
			{
				// check if this building can be built on this tile

				// can't build cities like that
				if (type.Name == "City") continue;
				// if the type is a farm, and the tile is polluted, don't create the button
				if (type.Name == "Farm" && tile.Polluted == true) continue;

				// create a button for this facility and add it to the parent object
				// instantiate new button
				GameObject button = Instantiate(buttonTemplate) as GameObject;
				button.SetActive(true);

				// change text of the button
				button.GetComponent<BuildButton>().SetText(type.Name);

				// set parent of the button with the parent of the button template's parent 
				// (our content object)
				button.transform.SetParent(buttonTemplate.transform.parent, false);
			}
		}
		else
		{
			Debug.Log("There is no tile here!");
		}
	}

}
