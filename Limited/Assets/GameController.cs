using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameController instance;
	public PlayerInventory playerInventory;
	public TilemapInteraction tilemapInteraction;
	public int currentTurn = 0;

	private GameObject notificationBig;

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


		// load savegame (eventually)

		// ...

		// start next turn (or first turn)
		NextTurn();
	}
	public void NextTurn()
	{
		if (currentTurn == 0)
		{
			FirstTurn();
		}
		// if turn is a multiple of 3
		else if (currentTurn % 3 == 0)
		{
			NewCity();
		}

		// next turn code

		// ...

		// finally, increase turn counter
		currentTurn++;
	}

	private void FirstTurn()
	{
		/* First turn script */

		// activate notification and fade it out
		notificationBig = GameObject.FindWithTag("UI_NotificationBig");

		if (notificationBig != null)
		{
			BigNotificationAPI notificationBig_script = notificationBig.GetComponent<BigNotificationAPI>(); // get the script attached to the notification object

			if (notificationBig_script != null)
			{
				notificationBig_script.SetText("Choose an initial location for your city.");
				notificationBig_script.FadeOut();
			}
		}

		NewCity();
	}

	private void NewCity()
	{
		// Get all possible tiles to found a city
		var possibleLocations = GameTiles.instance.GetPossibleCityTiles();

		// highlight them in green
		foreach (EnvironmentTile eTile in possibleLocations)
		{
			tilemapInteraction.HighlightTile(eTile);
		}
	}
}


