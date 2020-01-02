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
	public Texture2D facilitiesTileset;

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
			button.SetActive(true);
			BuildButton btnScript = button.GetComponent<BuildButton>();

			// change text of the button
			//btnScript.SetText(type.Name);

			// change the icon of the button
			// get all sliced tiles from our tileset
			Sprite[] tileSprites = Resources.LoadAll<Sprite>(facilitiesTileset.name);
			Sprite matchingSprite = tileSprites[0];

			// find the sprite 
			foreach (Sprite sprite in tileSprites)
			{
				if (sprite.name == type.SpriteName)
				{
					matchingSprite = sprite;
					break;
				}
			}

			btnScript.SetImage(matchingSprite);
			btnScript.SetType(type);
			btnScript.Enable(type.IsBuildable(firstPosition));

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
				BuildButton buildButton = button.GetComponent<BuildButton>();
				var type = buildButton.GetTileType();
				
				// hide/show buttons according to if they are buildable on this tile
				buildButton.Enable(type.IsBuildable(tile));
			}

			lastTileClicked = tile;
		}
		else
		{
			Debug.Log("There is no tile here!");
		}
	}

	public void ButtonClicked(FacilitiesTileType type)
	{
		Vector3Int pos = lastTileClicked.LocalPlace;

		// builds new tile
		GameTiles.instance.BuildFacility(type, pos);

		// close dialog box
		var dialogBox = GameObject.FindWithTag("UI_BuildDialogBox").GetComponent<BuildDialogBoxAPI>();
		dialogBox.Enabled(false);
	}

}
