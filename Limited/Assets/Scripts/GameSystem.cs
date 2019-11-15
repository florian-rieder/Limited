using UnityEngine;
using System.Collections.Generic;

public class GameSystem : MonoBehaviour
{
	/* Object that permits use of different non specific "utility" functions 
       that can be used in any other part of the code */

	public static void DumpToConsole(object obj)
	{
		/* debug function: displays an object to the console */
		var output = JsonUtility.ToJson(obj, true);
		Debug.Log(output);
	}

	public static bool xor(bool a, bool b)
	{
		/* exclusive or (xor) logical operator. */
		return (a && !b) || (!a && b);
	}

	// in Vector3Int
	public static int ManhattanDistance(Vector3Int a, Vector3Int b)
	{
		checked
		{
			return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
		}
	}

	public static List<Vector3Int> FindInRange(Vector3Int tilePosition, int range)
	{
		/* Get all tiles within manhattan distance of an origin tile */

		List<Vector3Int> tilesInRange = new List<Vector3Int>();

		int minX = tilePosition.x - range;
		int maxX = tilePosition.x + range;
		int minY = tilePosition.y - range;
		int maxY = tilePosition.y + range;

		for (int x = minX; x <= maxX; x++)
		{
			for (int y = minY; y <= maxY; y++)
			{
				Vector3Int thisPos = new Vector3Int(x, y, 0);
				if (ManhattanDistance(tilePosition, thisPos) <= range)
				{
					tilesInRange.Add(thisPos);
				}
			}
		}


		return tilesInRange;
	}
}
