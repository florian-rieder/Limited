using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public static GameController instance;
	public PlayerInventory playerInventory;
	public BigNotificationAPI notificationBig;
	public TilemapInteraction tilemapInteraction;
	public TimerDisplay timerDisplay;
	// time in [s] that has elapsed
	private float timer = 0f;
	// controls if we count the time elapsed
	private bool updateTimer = false;
	private float nextGrowthTime = 0f;

	private bool firstCityTutorialEnabled = true;
	private bool firstTimerLaunched = false;

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


		StartGame();
	}

	void Update()
	{
		// do stuff

		// ...

		if (!firstTimerLaunched && GameTiles.instance.GetCities().Count == 1)
		{
			bool cityNeedsSatisfied = true;

			if (firstCityTutorialEnabled)
			{
				foreach (KeyValuePair<string, int> entry in playerInventory.getCount())
				{
					//Debug.Log(entry.Key + ": " + entry.Value);
					if (entry.Value < 0)
					{
						cityNeedsSatisfied = false;
						break;
					}
				}
			}

			//Debug.Log(cityNeedsSatisfied);

			if (cityNeedsSatisfied)
			{
				nextGrowthTime = GetNextCityGrowthTime();
				EnableTimer(true);

				firstTimerLaunched = true;
			}
		}

		// count the time that has elapsed
		if (updateTimer)
		{
			if (timer > nextGrowthTime)
			{
				EnableTimer(false);
				ResetTimer();
				NewCity();
			}

			timer += Time.deltaTime;
		}
	}

	private void StartGame()
	{
		/* First turn script */
		notificationBig.SetText("Choose an initial location for your city.");
		notificationBig.FadeOut();

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

	public void CityBuilt()
	{
		if (GameTiles.instance.GetCities().Count == 1)
		{
			// first turn: don't launch the timer yet, we'll wait until
			// the player has figured out things and has successfully supplied
			// what was necessary to feed its first city.
			return;
		}
		/* triggers when a city is built on the map */
		nextGrowthTime = GetNextCityGrowthTime();
		EnableTimer(true);
	}

	private float GetNextCityGrowthTime()
	{
		// parameters
		float maxTime = 15f;
		float steepness = 9f;

		float time = 0f;

		int cities = GameTiles.instance.GetCities().Count;

		time = maxTime / (1 + cities / steepness);

		return time;
	}

	public void EnableTimer(bool value)
	{
		Debug.Log("Enable timer: " + value);
		timerDisplay.Enable(value);
		updateTimer = value;
	}

	public void ResetTimer()
	{
		timer = 0f;
	}

	public float GetTimeRemaining()
	{
		return nextGrowthTime - timer;
	}
}


