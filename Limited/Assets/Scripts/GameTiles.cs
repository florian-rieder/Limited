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
	public Texture2D facilitiesTileset;

	public GameObject healthBarTemplate;

	public Dictionary<Vector3Int, EnvironmentTile> environmentTiles;
	public Dictionary<Vector3Int, FacilityTile> facilitiesTiles;

	public List<string> EnvironmentResourceNames;
	public List<string> FacilitiesResourceNames;

	[SerializeField]
	private CameraController cameraCtrl;

	public Color PollutionColor;

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

	public EnvironmentTileTypeRoot GetEnvironmentTypes()
	{
		// Get tile type definitions from JSON file
		string environmentJSONContents = environmentTileTypesJSON.text;
		// Get array of types
		EnvironmentTileTypeRoot environmentRoot = JsonUtility.FromJson<EnvironmentTileTypeRoot>(environmentJSONContents);

		return environmentRoot;
	}
	public FacilitiesTileTypeRoot GetFacilitiesTypes()
	{
		string facilitiesJSONContents = facilitiesTileTypesJSON.text;
		FacilitiesTileTypeRoot facilitiesRoot = JsonUtility.FromJson<FacilitiesTileTypeRoot>(facilitiesJSONContents);

		return facilitiesRoot;
	}

	private void GetWorldTiles()
	{
		/* initialize tiles dictionaries */

		environmentTiles = new Dictionary<Vector3Int, EnvironmentTile>();
		facilitiesTiles = new Dictionary<Vector3Int, FacilityTile>();


		// Get types
		EnvironmentTileTypeRoot environmentRoot = GetEnvironmentTypes();
		FacilitiesTileTypeRoot facilitiesRoot = GetFacilitiesTypes();

		// Generate resource names lists from the first type that is in our types lists
		foreach (KeyValuePair<string, int> type in environmentRoot.tileTypes[0].GenerateResourcesDictionary())
		{
			EnvironmentResourceNames.Add(type.Key);
		}
		foreach (KeyValuePair<string, int> type in facilitiesRoot.tileTypes[0].GenerateResourcesDictionary())
		{
			FacilitiesResourceNames.Add(type.Key);
		}

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
					Resources = tileType.GenerateResourcesDictionary()
				};

				// add the tile representation to our dictionnary of tiles
				environmentTiles.Add(environmentTile.LocalPlace, environmentTile);
			}
			// FACILITY TILES
			if (facilitiesTilemap.HasTile(localPlace))
			{
				var TileBase = facilitiesTilemap.GetTile(localPlace);
				FacilitiesTileType tileType = facilitiesRoot.FindType(TileBase.name); // find the type of this tile by the name of its sprite
				HealthBar healthBar = null;

				if (tileType.Extractor)
				{
					// Attach healthbar if facility is an extractor

					var bar = Instantiate(healthBarTemplate);
					bar.SetActive(true);
					HealthBar script = bar.GetComponent<HealthBar>();
					script.MoveTo(localPlace);
					healthBar = script;

				}

				var facilityTile = new FacilityTile
				{
					LocalPlace = localPlace,
					TileBase = facilitiesTilemap.GetTile(localPlace),
					TilemapMember = facilitiesTilemap,

					// Here, we represent consumption by negative values for its resource
					// and we represent production by positive values
					Name = tileType.Name,
					Resources = tileType.GenerateResourcesDictionary(),
					Extractor = tileType.Extractor,
					HealthBar = healthBar,
					PollutionRadius = tileType.PollutionRadius
				};

				facilitiesTiles.Add(facilityTile.LocalPlace, facilityTile);
			}
		}
	}

	public void BuildFacility(FacilitiesTileType facilityType, Vector3Int position)
	{
		// get all sliced tiles from our tileset
		Sprite[] tileSprites = Resources.LoadAll<Sprite>(facilitiesTileset.name);
		Sprite matchingSprite = tileSprites[0];

		// find the sprite 
		foreach (Sprite sprite in tileSprites)
		{
			if (sprite.name == facilityType.SpriteName)
			{
				matchingSprite = sprite;
				break;
			}
		}

		var tile = ScriptableObject.CreateInstance<Tile>();
		tile.sprite = matchingSprite;

		facilitiesTilemap.SetTile(position, tile);

		HealthBar healthBar = null;

		// Attach healthbar if facility is an extractor
		if (facilityType.Extractor)
		{
			var bar = Instantiate(healthBarTemplate);
			bar.SetActive(true);
			HealthBar healthBarController = bar.GetComponent<HealthBar>();
			healthBarController.MoveTo(position);
			healthBar = healthBarController;
		}

		// create virtual representation of the new tile
		var facilityTile = facilityType.GenerateTile(environmentTilemap, position, healthBar);

		// apply pollution
		if (facilityTile.PollutionRadius > 0)
		{
			List<Vector3Int> pollutedTiles = GameSystem.FindInRange(facilityTile.LocalPlace, facilityTile.PollutionRadius);

			// iterate through all tiles in the pollution radius
			foreach (Vector3Int pos in pollutedTiles)
			{
				EnvironmentTile tileToPollute;
				if (GameTiles.instance.environmentTiles.TryGetValue(pos, out tileToPollute))
				{
					if (tileToPollute.Name != "Water")
					{
						tileToPollute.Pollute();
					}
				}
			}
		}

		facilitiesTiles.Add(facilityTile.LocalPlace, facilityTile);

		// start screenshake
		if(facilityTile.Name == "City"){
			cameraCtrl.TriggerShake(0.2f);
		}
		else {
			cameraCtrl.TriggerShake(0.1f);
		}
	}

	public List<EnvironmentTile> GetPossibleCityTiles()
	{
		List<EnvironmentTile> possibleLocations = new List<EnvironmentTile>();

		// find by sprite name... I know it's not very elegant
		FacilitiesTileType cityType = GetFacilitiesTypes().FindType("Tileset_facilities_0");

		var existingCities = GetCities();

		// if first city, check all the tiles that are buildable for a city in the map
		if (existingCities.Count == 0)
		{
			foreach (KeyValuePair<Vector3Int, EnvironmentTile> environmentEntry in environmentTiles)
			{
				// initialize loop variables
				var position = environmentEntry.Key;
				EnvironmentTile eTile = environmentEntry.Value;

				if (cityType.IsBuildable(eTile))
				{
					possibleLocations.Add(eTile);
				}
			}
		}
		// if there is at least one city existing, restrain search to adjacent tiles
		else
		{
			List<Vector3Int> positionsChecked = new List<Vector3Int>();

			foreach (KeyValuePair<Vector3Int, FacilityTile> entry in existingCities)
			{
				var cityPos = entry.Key;
				var city = entry.Value;

				//for all adjacent tiles to this one
				for (int x = -1; x <= 1; x++)
				{
					for (int y = -1; y <= 1; y++)
					{
						// restrict to only directly adjacent tiles (with a side touching)
						if (GameSystem.xor(Mathf.Abs(x) == 1, Mathf.Abs(y) == 1) && !(x == 0 && y == 0))
						{
							Vector3Int currPos = new Vector3Int(cityPos.x + x, cityPos.y + y, cityPos.z);

							if (!positionsChecked.Contains(currPos))
							{
								// Add this position to our list of already checked tiles in order to avoid redundant checks
								positionsChecked.Add(currPos);
								EnvironmentTile eTile;

								// if there is a tile here
								if (environmentTiles.TryGetValue(currPos, out eTile))
								{
									// if the city can be built here
									if (cityType.IsBuildable(eTile))
									{
										possibleLocations.Add(eTile);
									}
								}
							}
						}

					}
				}
			}
		}


		return possibleLocations;
	}

	public Dictionary<Vector3Int, FacilityTile> GetCities()
	{
		var cities = new Dictionary<Vector3Int, FacilityTile>();
		foreach (KeyValuePair<Vector3Int, FacilityTile> entry in facilitiesTiles)
		{
			if (entry.Value.Name == "City") cities[entry.Key] = entry.Value;
		}
		return cities;
	}
}