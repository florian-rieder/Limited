/* script attached to the Grid gameObject containing our 2 tilemaps */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTiles : MonoBehaviour
{
	public static GameTiles instance;
	public Tilemap environmentTilemap;
	public Tilemap facilitiesTilemap;
	public TextAsset environmentTileTypesJSON;
	public TextAsset facilitiesTileTypesJSON;

	public Dictionary<Vector3Int, EnvironmentTile> environmentTiles;
	public Dictionary<Vector3Int, FacilityTile> facilitiesTiles;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		GetWorldTiles();
	}

	public EnvironmentTileTypeRoot GetEnvironmentTypes(){
		// Get tile type definitions from JSON file
		string environmentJSONContents = environmentTileTypesJSON.text;
		// Get array of types
		EnvironmentTileTypeRoot environmentRoot = JsonUtility.FromJson<EnvironmentTileTypeRoot>(environmentJSONContents);

		return environmentRoot;
	}
	public FacilitiesTileTypeRoot getFacilitiesTypes(){
		string facilitiesJSONContents = facilitiesTileTypesJSON.text;
		FacilitiesTileTypeRoot facilitiesRoot = JsonUtility.FromJson<FacilitiesTileTypeRoot>(facilitiesJSONContents);
		
		return facilitiesRoot;
	}

	// Use this for initialization
	private void GetWorldTiles()
	{
		environmentTiles = new Dictionary<Vector3Int, EnvironmentTile>();
		facilitiesTiles = new Dictionary<Vector3Int, FacilityTile>();

		// Get tile type definitions from JSON file
		string environmentJSONContents = environmentTileTypesJSON.text;
		// Get array of types
		EnvironmentTileTypeRoot environmentRoot = JsonUtility.FromJson<EnvironmentTileTypeRoot>(environmentJSONContents);

		string facilitiesJSONContents = facilitiesTileTypesJSON.text;
		FacilitiesTileTypeRoot facilitiesRoot = getFacilitiesTypes();

		// Add game tiles to our tile dictionary for further referencing, should it be the case (spoiler alert: it will!)
		// iterate through all tiles in the tilemap
		foreach (Vector3Int pos in environmentTilemap.cellBounds.allPositionsWithin)
		{
			var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

			// ENVIRONMENT TILES
			// if there is a tile here...
			if (environmentTilemap.HasTile(localPlace))
			{
				// Find its type according to our parsed JSON data
				var TileBase = environmentTilemap.GetTile(localPlace);
				EnvironmentTileType tileType = environmentRoot.FindType(TileBase.name); // find the type of this tile by the name of its sprite

				// Assign type defined variables to actual tile representation that is to be stored in our dictionary of tiles
				var environmentTile = new EnvironmentTile
				{
					LocalPlace = localPlace,
					TileBase = environmentTilemap.GetTile(localPlace),
					TilemapMember = environmentTilemap,

					// Amount of resources available in this tile
					Name = tileType.Name,
					Oil = tileType.Oil,
					Coal = tileType.Coal,
					Wood = tileType.Wood,
				};

				// add the tile representation to our dictionnary of tiles
				environmentTiles.Add(environmentTile.LocalPlace, environmentTile);
				
			}
			// FACILITY TILES
			if (facilitiesTilemap.HasTile(localPlace))
			{
				var TileBase = facilitiesTilemap.GetTile(localPlace);
				FacilitiesTileType tileType = facilitiesRoot.FindType(TileBase.name); // find the type of this tile by the name of its sprite

				var facilityTile = new FacilityTile
				{
					LocalPlace = localPlace,
					TileBase = facilitiesTilemap.GetTile(localPlace),
					TilemapMember = environmentTilemap,

					// Here, we represent consumption by negative values for its resource
					// and we represent production by positive values
					Name = tileType.Name,
					Oil = tileType.Oil,
					Coal = tileType.Coal,
					Wood = tileType.Wood,
					Power = tileType.Power,
					Goods = tileType.Goods,
					Food = tileType.Food,

					PollutionRadius = tileType.PollutionRadius

				};

				facilitiesTiles.Add(facilityTile.LocalPlace, facilityTile);
			}
		}
		
	}

	
}