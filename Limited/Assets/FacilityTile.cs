using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class FacilityTile
{
	public Vector3Int LocalPlace { get; set; }
	public Tilemap TilemapMember { get; set; }

	public TileBase TileBase { get; set; }

	public string Name { get; set; }

	public Dictionary<string, int> Resources { get; set; }
	
	public int PollutionRadius { get; set; }
	public bool Extractor { get; set; }
}

// Classes used to retrieve tile data from JSON file
[Serializable]
public class FacilitiesTileType
{
	public string Name;
	public string SpriteName;

	// consumption of resources is represented by a negative value
	// production by positive value
	public int Oil;
	public int Coal;
	public int Wood;
	public int Metal;
	public int Power;
	public int Goods;
	public int Food;

	public int PollutionRadius;
	public bool Extractor;

	public Dictionary<string, int> GenerateResourcesDictionary()
	{
		return new Dictionary<string, int>{
			{"Oil", Oil},
			{"Coal", Coal},
			{"Wood", Wood},
			{"Metal", Metal},
			{"Power", Power},
			{"Goods", Goods},
			{"Food", Food}
		};
	}

	public bool IsBuildable(EnvironmentTile tile)
	{
		PlayerInventory playerInventory = GameController.instance.playerInventory;
		Dictionary<string, int> inventory = playerInventory.getCount();
		Dictionary<string, int> typeResourcesDictionary = GenerateResourcesDictionary();

		bool canBuild = true;

		// check if the player has the resources that are needed to build this facility
		foreach (KeyValuePair<string, int> resource in inventory)
		{
			string name = resource.Key;
			int availableAmount = resource.Value;

			int typeAmountForThisRessource = typeResourcesDictionary[name];

			if (Extractor && GameTiles.instance.EnvironmentResourceNames.Contains(name))
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
			if (Name == "Farm" && tile.Polluted == true) canBuild = false;
		}

		return canBuild;
	}
}

[Serializable]
public class FacilitiesTileTypeRoot
{
	/* Root class used because the jsonUtility needs the JSON to represent 
       a class (can't parse an array of objects) */
	public FacilitiesTileType[] tileTypes;

	public FacilitiesTileType FindType(string spriteName)
	{
		/* finds the type of a certain tile based on its sprite name
		   returns null if there is no type corresponding to the given name */

		FacilitiesTileType returnType = null;
		for (int i = 0; i < tileTypes.Length; i++)
		{
			var type = tileTypes[i];

			if (spriteName == type.SpriteName)
			{
				returnType = type;
				break;
			}
		}
		return returnType;
	}
}