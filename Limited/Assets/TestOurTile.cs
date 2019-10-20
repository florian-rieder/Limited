using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestOurTile : MonoBehaviour {
	private Tile _tile;
	public Tilemap tilemap;
	
	// Update is called once per frame
	private void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var tilePos = tilemap.WorldToCell(point);

			var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles

			if (tiles.TryGetValue(tilePos, out _tile)) 
			{
				Debug.Log("Tile " + _tile.Name + ": " + _tile.TileBase.ToString());
				_tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
				_tile.TilemapMember.SetColor(_tile.LocalPlace, Color.green);
			}
		}
	}
}