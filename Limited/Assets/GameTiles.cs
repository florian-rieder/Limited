using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTiles : MonoBehaviour {
	public static GameTiles instance;
	public Tilemap Tilemap;

	public Dictionary<Vector3, Tile> tiles;

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

	// Use this for initialization
	private void GetWorldTiles () 
	{
        tiles = new Dictionary<Vector3, Tile>();
		foreach (Vector3Int pos in Tilemap.cellBounds.allPositionsWithin)
		{
			var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

			if (!Tilemap.HasTile(localPlace)) continue;
			var tile = new Tile
			{
				LocalPlace = localPlace,
				TileBase = Tilemap.GetTile(localPlace),
				TilemapMember = Tilemap,
				Name = localPlace.x + "," + localPlace.y,
			};
			
			tiles.Add(tile.LocalPlace, tile);
		}
	}
}