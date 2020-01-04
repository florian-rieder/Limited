using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PollutionController : MonoBehaviour
{
    public Tilemap pollutionTilemap;
    public RuleTile pollutionTile;

	public void PolluteTile(Vector3Int realPos)
	{
		// get the 4 tiles in this grid that are contained in the real tile we want to pollute
		var tilesBounds = new BoundsInt(0, 0, 0, 2, 2, 1);

		foreach (var pos in tilesBounds.allPositionsWithin)
		{
            // get correct position, knowing that this grid units are half of the real grid units
			var metaPos = new Vector3Int(
				realPos.x * 2 + pos.x,
				realPos.y * 2 + pos.y,
				realPos.z
			);

            pollutionTilemap.SetTile(metaPos, pollutionTile);
		}
	}
}
