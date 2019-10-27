using UnityEngine.Tilemaps;
using UnityEngine;

public class Tile
{
    public Vector3Int LocalPlace { get; set; }

    public TileBase TileBase { get; set; }

    public Tilemap TilemapMember { get; set; }

    public string Name { get; set; }
}
