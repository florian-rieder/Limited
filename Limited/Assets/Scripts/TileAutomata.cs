using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TileAutomata : MonoBehaviour
{
	[Range(0, 100)]
	public int initChance;
	[Range(1, 8)]
	public int birthLimit;
	[Range(1, 8)]
	public int deathLimit;
	public int minBuildableTiles;
	public int maxIterations;

	private int[,] terrainMap;
	public Vector3Int tilemapSize;

	public Tilemap tilemap;
	public RuleTile groundTile;
	public Tile waterTile;

	int width;
	int height;

	public Sprite plainTileSprite;

	public TileVarietiesObject[] tileVarieties;

	public void doSim(int iterations)
	{
		clearMap(false);
		width = tilemapSize.x;
		height = tilemapSize.y;

		// generate a new map while there are not a minimum of buildable tiles
		// (prevents generation of maps that are too small)
		for (int idx = 0; GetBuildableTilesCount() < minBuildableTiles && idx < maxIterations; idx++)
		{
			// initialize grid
			if (terrainMap == null)
			{
				terrainMap = new int[width, height];
				initPos();
			}

			// run simulation for n iterations
			for (int i = 0; i < iterations; i++)
			{
				terrainMap = genTilePos(terrainMap);
			}

			// apply to tilemap
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					var newPos = new Vector3Int(-x + width / 2, -y + height / 2, 0);

					if (terrainMap[x, y] == 1)
					{
						tilemap.SetTile(newPos, groundTile);
					} 
					else
					{
						tilemap.SetTile(newPos, waterTile);
					}
				}
			}
		}

		// convert rule tiles to tiles, because we want to then modify the terrain created using the rule
		// tiles. If we didn't, each resource tile we are going to randomly insert will disrupt the textures connections
		// of the terrain
		ConvertRuleTilesToTiles();

		var resourcesScattered = new Dictionary<string, int>();

		// scatter resources around the map
		foreach (var variety in tileVarieties)
		{
			// initialize a registry of the number of this resource placed on the map
			resourcesScattered.Add(variety.name, 0);

			foreach (var pos in tilemap.cellBounds.allPositionsWithin)
			{
				// if there is no tile here, skip
				if (!tilemap.HasTile(pos)) continue;

				// if the tile is a plain
				if (tilemap.GetSprite(pos).name != plainTileSprite.name) continue;

				// insert resource in tilemap with a certain chance
				int rando = Random.Range(0, 1000);

				if (variety.chance >= rando)
				{
					// create a new regular tile with our resource's sprite
					var newTile = ScriptableObject.CreateInstance<Tile>();
					newTile.sprite = variety.sprite;

					// replace the tile with our new tile
					tilemap.SetTile(pos, newTile);

					// register that one of this ressource has been placed
					resourcesScattered[variety.name]++;
				}
			}

			// insure a minimal amount of resources on the map
			if (resourcesScattered[variety.name] >= variety.minimalQuantity) continue;

            int max_iterations = 20;

			for (int i = 0; i < variety.minimalQuantity - resourcesScattered[variety.name]; i++)
			{
                int iteration = 0;
				// place another tile of this resource randomly on the map
				while (true)
				{
					// randomly select a tile within the bounds of the map
					var randomPosition = GameSystem.RandomInsideBoundsInt(tilemap.cellBounds);

					if (!tilemap.HasTile(randomPosition)) continue;

					if (tilemap.GetSprite(randomPosition).name == plainTileSprite.name)
					{
						// create a new regular tile with our resource's sprite
						var newTile = ScriptableObject.CreateInstance<Tile>();
						newTile.sprite = variety.sprite;

						// replace the tile with our new tile
						tilemap.SetTile(randomPosition, newTile);
						break;
					}

                    if (iteration >= max_iterations) break;

                    iteration++;
				}
			}
		}
	}

	public int[,] genTilePos(int[,] oldMap)
	{
		int[,] newMap = new int[width, height];
		BoundsInt neighborBounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int neighbors = 0;

				// count neighbors
				foreach (Vector3Int adjacentPosition in neighborBounds.allPositionsWithin)
				{
					if (adjacentPosition.x == 0 && adjacentPosition.y == 0) continue;

					// verify if tile is not out of bounds
					if (
						adjacentPosition.x + x < width && adjacentPosition.x + x >= 0 &&
						adjacentPosition.y + y < height && adjacentPosition.y + y >= 0
					)
					{
						neighbors += oldMap[x + adjacentPosition.x, y + adjacentPosition.y];
					}
				}

				// test against birth and death limits

				// if cell is alive, check if it should continue living or die
				if (oldMap[x, y] == 1)
				{
					newMap[x, y] = neighbors < deathLimit ? 0 : 1;
				}

				// if cell is dead, check if it should live or stay dead
				if (oldMap[x, y] == 0)
				{
					newMap[x, y] = neighbors > birthLimit ? 1 : 0;
				}
			}
		}

		return newMap;
	}

	public void initPos()
	{
		// initialize grid with random values
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				terrainMap[x, y] = Random.Range(1, 101) < initChance ? 1 : 0;
			}
		}
	}

	public void clearMap(bool complete)
	{
		tilemap.ClearAllTiles();

		if (complete)
		{
			terrainMap = null;
		}
	}

	private void ConvertRuleTilesToTiles()
	{
		/* converts all ruletiles in the tilemap into regular tiles, so that we 
           can later change tiles in the tilemap without breaking tile connections */

		Dictionary<Vector3Int, Sprite> tilesSprites = new Dictionary<Vector3Int, Sprite>();

		// get all sprites on the tilemap
		foreach (var pos in tilemap.cellBounds.allPositionsWithin)
		{
			if (!tilemap.HasTile(pos)) continue;

			var tile = tilemap.GetTile(pos);
			tilesSprites.Add(pos, tilemap.GetSprite(pos));
		}

		foreach (KeyValuePair<Vector3Int, Sprite> entry in tilesSprites)
		{
			// create new regular tile with old ruletile sprite
			var newTile = ScriptableObject.CreateInstance<Tile>();
			newTile.sprite = entry.Value;

			// replace tile
			tilemap.SetTile(entry.Key, newTile);
		}
	}

	private int GetBuildableTilesCount()
	{
		int count = 0;

		foreach (var pos in tilemap.cellBounds.allPositionsWithin)
		{
			if (!tilemap.HasTile(pos)) continue;
			if (tilemap.GetSprite(pos).name == plainTileSprite.name) count++;
		}

		return count;
	}
}

[System.Serializable]
public class TileVarietiesObject
{
	public string name;
	public Sprite sprite;
	public int chance;
	public int minimalQuantity;
}