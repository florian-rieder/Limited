using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class ButtonListControl : MonoBehaviour
{
	[SerializeField]
	private GameObject buttonTemplate;
	private Tilemap facilitiesTilemap;

	private List<GameObject> buttons = new List<GameObject>();

	private EnvironmentTile lastTileClicked;
	private FacilitiesTileTypeRoot facilitiesTypes;

	public void Initialize(EnvironmentTile firstPosition)
	{
		facilitiesTypes = GameTiles.instance.GetFacilitiesTypes();

		foreach (FacilitiesTileType type in facilitiesTypes.tileTypes)
		{
			// can't build cities
			if (type.Name == "City") continue;

			// create a button for this facility and add it to the parent object
			// instantiate new button
			GameObject button = Instantiate(buttonTemplate) as GameObject;
			BuildButton btnScript = button.GetComponent<BuildButton>();

			// change text of the button
			btnScript.SetText(type.Name);
			btnScript.SetType(type);

			if (btnScript.IsBuildable(firstPosition))
			{
				button.SetActive(true);
			}
			else
			{
				button.SetActive(false);
			}

			// set parent of the button with the parent of the button template's parent 
			// (our content object)
			button.transform.SetParent(buttonTemplate.transform.parent, false);

			buttons.Add(button);
		}
	}

	public void UpdateButtons(EnvironmentTile tile)
	{
		// if there is no tile there is nothing that can be built
		if (tile != null)
		{
			// check which facilities can be built on the specified tile
			foreach (GameObject button in buttons)
			{
				// hide/show buttons according to if they are buildable on this tile
				if (button.GetComponent<BuildButton>().IsBuildable(tile))
				{
					button.SetActive(true);
				}
				else
				{
					button.SetActive(false);
				}
			}

			lastTileClicked = tile;
		}
		else
		{
			Debug.Log("There is no tile here!");
		}
	}

	public void ButtonClicked(FacilitiesTileType type){
		Vector3Int pos = lastTileClicked.LocalPlace;

		// builds new tile
		GameTiles.instance.BuildFacility(type, pos);

		// close dialog box
		var dialogBox = GameObject.FindWithTag("UI_BuildDialogBox").GetComponent<BuildDialogBoxAPI>();
		dialogBox.Enabled(false);
	}

}
