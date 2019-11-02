using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class PlayerInventory : MonoBehaviour
{

	public Dictionary<string, int> getCount()
	{
		var inventory = new Dictionary<string, int>();
		string[] resourceNames = new string[7] { "Oil", "Coal", "Wood", "Metal", "Power", "Goods", "Food" };

		// initialize dictionary
		foreach (string name in resourceNames)
		{
			inventory[name] = 0;
		}

		// get dictionary of facility tiles
		var facilityTiles = GameTiles.instance.facilitiesTiles;

		foreach (KeyValuePair<Vector3Int, FacilityTile> entry in facilityTiles)
		{
			var tile = entry.Value;

			foreach (string name in resourceNames)
			{
				inventory[name] += (int)typeof(FacilityTile).GetProperty(name).GetValue(tile);
			}
		}

		return inventory;
	}
}