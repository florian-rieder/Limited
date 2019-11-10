using System;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;
[Serializable]
public class EnvironmentTile
{
	public Vector3Int LocalPlace { get; set; }
	public TileBase TileBase { get; set; }
	public Tilemap TilemapMember { get; set; }

	public string Name { get; set; }
	public Dictionary<string, int> Resources { get; set; }
	public bool Polluted { get; set; }

	public bool IsHighlighted()
	{
		return TilemapMember.GetColor(LocalPlace) == Color.green;
	}

	public void Pollute()
	{
		//Color PollutionColor = new Color(0.4f, 0.3f, 0.4f, 1f);
		Polluted = true;

		// tint the tile to our pollution color
		TilemapMember.SetTileFlags(LocalPlace, TileFlags.None);
		TilemapMember.SetColor(LocalPlace, GameTiles.instance.PollutionColor);
	}
}

// Classes used to retrieve tile data from JSON file
[Serializable]
public class EnvironmentTileType
{
	public string Name;
	public string[] SpriteNames;

	public int Oil;
	public int Coal;
	public int Wood;
	public int Metal;

	public Dictionary<string, int> GenerateResourcesDictionary()
	{
		return new Dictionary<string, int>{
			{"Oil", Oil},
			{"Coal", Coal},
			{"Wood", Wood},
			{"Metal", Metal},
		};
	}
}

// Root class used because the jsonUtility needs the JSON to represent 
// a class (can't parse an array of objects)
[Serializable]
public class EnvironmentTileTypeRoot
{
	public EnvironmentTileType[] tileTypes;

	public EnvironmentTileType FindType(string spriteName)
	{
		/* finds the type of a certain tile based on its sprite name
		   returns null if there is no type corresponding to the given name */

		EnvironmentTileType returnType = null;
		foreach (EnvironmentTileType type in tileTypes)
		{
			foreach (string name in type.SpriteNames)
			{
				if (spriteName == name)
				{
					returnType = type;
					break;
				}
			}
		}
		return returnType;
	}

	public EnvironmentTileType FindTypeByName(string name)
	{
		EnvironmentTileType returnType = null;
		foreach (EnvironmentTileType type in tileTypes)
		{
			if(type.Name == name){
				returnType = type;
				break;
			}
		}
		return returnType;
	}
}
