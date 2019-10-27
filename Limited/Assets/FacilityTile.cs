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

	public int Oil { get; set; }
	public int Coal { get; set; }
	public int Wood { get; set; }
	public int Power { get; set; }
	public int Goods { get; set; }
	public int Food { get; set; }

	public int PollutionRadius { get; set; }
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
	public int Power;
	public int Goods;
	public int Food;

	public int PollutionRadius;
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