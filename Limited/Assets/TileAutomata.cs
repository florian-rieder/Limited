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
	[Range(1, 10)]
	public int iterations;

	private int[,] terrainMap;
	public Vector3Int tilemapSize;

	public Tilemap tilemap;
	public RuleTile groundTile;

	int width;
	int height;

	public void doSim(int iterations)
	{
		clearMap(false);
		width = tilemapSize.x;
		height = tilemapSize.y;

		// initialize grid
		if (terrainMap == null)
		{
			terrainMap = new int[width, height];
			initPos();
		}

		// run simulation for numRepet iterations
		for (int i = 0; i < iterations; i++)
		{
			terrainMap = genTilePos(terrainMap);
		}

        // apply to tilemap
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (terrainMap[x, y] == 1)
				{
					tilemap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), groundTile);
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

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			doSim(iterations);
		}

		if (Input.GetMouseButtonDown(1))
		{
			clearMap(true);
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
}
