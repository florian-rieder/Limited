using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class PlayerInventory : MonoBehaviour
{

	public Inventory countInventory()
	{
        var inventory = new Inventory();

		// get dictionary of facility tiles
		var facilityTiles = GameTiles.instance.facilitiesTiles;

		foreach (KeyValuePair<Vector3Int, FacilityTile> entry in facilityTiles)
		{
			var tile = entry.Value;

            inventory.Oil += tile.Oil;
            inventory.Coal += tile.Coal;
            inventory.Wood += tile.Wood;
            inventory.Power += tile.Power;
            inventory.Goods += tile.Goods;
            inventory.Food += tile.Food;
		}

        return inventory;
	}
}

public class Inventory
{
	public int Oil = 0;
	public int Coal = 0;
	public int Wood = 0;
	public int Power = 0;
	public int Goods = 0;
	public int Food = 0;
}