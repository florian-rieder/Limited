using UnityEngine;
public class GameSystem : MonoBehaviour
{
	/* Object that permits use of different non specific "utility" functions 
       that can be used in any other part of the code */
	public static GameSystem instance;


	public static void DumpToConsole(object obj)
	{
		/* debug function: displays an object to the console */
		var output = JsonUtility.ToJson(obj, true);
		Debug.Log(output);
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}
}
