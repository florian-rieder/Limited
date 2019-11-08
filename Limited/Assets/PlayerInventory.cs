using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{

	public Dictionary<string, int> getCount()
	{
		var inventory = new Dictionary<string, int>();
		List<string> resourceNames = GameTiles.instance.FacilitiesResourceNames;

		// initialize dictionary
		foreach (string name in resourceNames)
		{
			inventory[name] = 0;
		}

		// get dictionary of facility tiles
		var facilityTiles = GameTiles.instance.facilitiesTiles;

		// for each facility tile on the tilemap...
		foreach (KeyValuePair<Vector3Int, FacilityTile> entry in facilityTiles)
		{
			var tile = entry.Value;

			// iterate through the resource consumption/production and add its value to our total
			foreach (string name in resourceNames)
			{
				inventory[name] += tile.Resources[name];
			}
		}

		return inventory;
	}
}