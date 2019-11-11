using UnityEngine;

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
    public static int ManhattanDistance(Vector3Int a, Vector3Int b){
        checked {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }
}
