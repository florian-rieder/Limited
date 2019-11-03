using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TilemapInteraction : MonoBehaviour
{
	public Tilemap environmentTilemap;
	public Tilemap facilitiesTilemap;
	public BuildDialogBoxAPI dialogBox;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			// prevents clicking through UI
			if (EventSystem.current.IsPointerOverGameObject())
			{
				Debug.Log("Clicked on the UI");
			}
			else
			{
				EnvironmentTile _environmentTile;
				FacilityTile _facilityTile;

				// get the position of the tile being clicked on
				Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3Int tilePos = environmentTilemap.WorldToCell(point);

				var environmentTiles = GameTiles.instance.environmentTiles; // This is our Dictionary of tiles
				var facilitiesTiles = GameTiles.instance.facilitiesTiles;

				bool environmentTileHere = false;
				bool facilityTileHere = false;

				if (facilitiesTiles.TryGetValue(tilePos, out _facilityTile))
				{
					// debug
					Debug.Log(createFacilityLog(_facilityTile));

					facilityTileHere = true;
				}
				// if we have a tile registered at this position
				if (environmentTiles.TryGetValue(tilePos, out _environmentTile))
				{
					// Debug
					Debug.Log(createEnvironmentLog(_environmentTile));

					// turn tile green
					// _environmentTile.TilemapMember.SetTileFlags(_environmentTile.LocalPlace, TileFlags.None);
					// _environmentTile.TilemapMember.SetColor(_environmentTile.LocalPlace, Color.green);

					environmentTileHere = true;
				}

				if (environmentTileHere)
				{
					if (_environmentTile.Name != "Water")
					{
						if (!facilityTileHere)
						{
							doBuildDialogBox(tilePos);
						}
					}
				}
				else
				{
					if (dialogBox.IsOpen())
					{
						dialogBox.Enabled(false);
					}
				}


			}

		}
	}

	private void doBuildDialogBox(Vector3Int pos)
	{
		if (dialogBox.IsOpen())
		{
			dialogBox.Enabled(false);
		}
		else
		{
			dialogBox.Enabled(true);
			dialogBox.MoveTo(pos);
			dialogBox.UpdateButtons(pos);
		}
	}

	private string createFacilityLog(FacilityTile ft)
	{
		string str = "";
		str += "Name: " + ft.Name;
		str += "\nSprite: " + ft.TileBase.ToString();
		foreach (string resourceName in GameTiles.instance.FacilitiesResourceNames)
		{
			str += "\n" + resourceName + ": " + ft.Resources[resourceName];
		}
		str += "\nPollution radius: " + ft.PollutionRadius;

		return str;
	}

	private string createEnvironmentLog(EnvironmentTile et)
	{
		string str = "";
		str += "Name: " + et.Name;
		str += "\nSprite: " + et.TileBase.ToString();
		foreach (string resourceName in GameTiles.instance.EnvironmentResourceNames)
		{
			str += "\n" + resourceName + ": " + et.Resources[resourceName];
		}

		return str;
	}
}
