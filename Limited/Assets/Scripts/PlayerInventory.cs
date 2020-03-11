using UnityEngine;
using System.Collections;
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
			var facility = entry.Value;

			// check if the facility is working before adding its production to our total
			if ((facility.Extractor && facility.GroundHasResources()) || (!facility.Extractor && facility.IsWorking))
			{
				// iterate through the resource consumption/production and add its value to our total
				foreach (string resourceName in resourceNames)
				{
					inventory[resourceName] += facility.Resources[resourceName];
				}
			}
		}

		return inventory;
	}

	public bool hasCityNeeds()
	{
		var inventory = getCount();
		List<string> basicNeeds = new List<string>() { "Food", "Goods", "Power" };
		bool hasNeeds = true;

		foreach (KeyValuePair<string, int> entry in inventory)
		{
			if (basicNeeds.Contains(entry.Key) && entry.Value < 0)
			{
				hasNeeds = false;
				break;
			}
		}

		return hasNeeds;
	}

	public List<string> GetMissingBasicResources()
	{
		var inventory = getCount();
		List<string> basicNeeds = new List<string>() { "Food", "Goods", "Power" };
		List<string> missingResources = new List<string>();

		foreach (KeyValuePair<string, int> entry in inventory)
		{
			if (basicNeeds.Contains(entry.Key) && entry.Value < 0)
			{
				missingResources.Add(entry.Key);
			}
		}

		return missingResources;
	}
}