using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public static GameController instance;
	public float famineTimerBase = 30f; // duration of famine until the game is lost
	public float growthTimerBase = 60f;
	public PlayerInventory playerInventory;
	public BigNotificationAPI notificationBig;
	public TilemapInteraction tilemapInteraction;
	public TimerDisplay timerDisplay;
	public GameOverPanel gameOverPanel;
	public FamineTimerDisplay famineTimerDisplay;
	// time in [s] that has elapsed
	private float timer = 0f;
	private float famineTimer = 0f;
	// controls if we count the time elapsed
	private bool updateTimer = false;
	private bool updateFamineTimer = false;
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
					if (entry.Value < 0)
					{
						cityNeedsSatisfied = false;
						break;
					}
				}
			}

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

		bool hasCityNeeds = playerInventory.hasCityNeeds();

		if (firstTimerLaunched)
		{
			if (!hasCityNeeds && !updateFamineTimer)
			{
				EnableFamineTimer(true);
			}
			if (hasCityNeeds && updateFamineTimer)
			{
				EnableFamineTimer(false);
				ResetFamineTimer();
			}
		}

		if (updateFamineTimer)
		{

			if (famineTimer > famineTimerBase)
			{
				EnableFamineTimer(false);
				GameOver("The need for basic resources turned your city into chaos.");
			}

			famineTimer += Time.deltaTime;
		}
	}

	private void StartGame()
	{
		/* First turn script */
		notificationBig.SetText("Choose an initial location for your city.");
		notificationBig.FadeOut();

		NewCity();

		// start extracting resources every x second
		InvokeRepeating("ExtractResources", 0, 5);
	}

	private void NewCity()
	{
		// Get all possible tiles to found a city
		var possibleLocations = GameTiles.instance.GetPossibleCityTiles();

		if (possibleLocations.Count == 0)
		{
			GameOver("Your city ran out of space to expand on !");
		}

		// highlight them in green
		foreach (EnvironmentTile eTile in possibleLocations)
		{
			tilemapInteraction.HighlightTile(eTile);
		}
	}

	public void CityBuilt()
	{
		/* triggers when a city is built on the map */

		if (GameTiles.instance.GetCities().Count == 1)
		{
			// first turn: don't launch the timer yet, we'll wait until
			// the player has figured out things and has successfully supplied
			// what was necessary to feed its first city.
			return;
		}

		nextGrowthTime = GetNextCityGrowthTime();
		EnableTimer(true);
	}

	private float GetNextCityGrowthTime()
	{
		// parameters
		float steepness = 10f;

		float time = 0f;

		int cities = GameTiles.instance.GetCities().Count;

		// get the time before the next expansion of the city
		// in function of the number of cities owned and our parameters
		time = growthTimerBase / (1 + cities / steepness);

		return time;
	}

	private void ExtractResources()
	{
		var facilities = GameTiles.instance.facilitiesTiles;

		// for each facility
		foreach (KeyValuePair<Vector3Int, FacilityTile> entry in facilities)
		{
			var facility = entry.Value;

			// if the facility extracts natural resources
			if (facility.Extractor)
			{
				// extract resources
				facility.Extract();
			}
		}
	}
	public void EnableTimer(bool value)
	{
		timerDisplay.Enable(value);
		updateTimer = value;
	}

	public void ResetTimer()
	{

		timer = 0f;
	}

	public void EnableFamineTimer(bool value)
	{
		updateFamineTimer = value;
		famineTimerDisplay.Enable(value);
	}
	public void ResetFamineTimer()
	{
		famineTimer = 0f;
	}
	public float GetFamineTimeRemaining()
	{
		return famineTimerBase - famineTimer;
	}
	public float GetTimeRemaining()
	{
		return nextGrowthTime - timer;
	}
	private void GameOver(string reason = "No reason specified.")
	{
		Debug.Log("Game Over.");

		// hide selector
		TileSelector.instance.gameObject.SetActive(false);

		// open game over panel
		gameOverPanel.SetReason(reason);
		gameOverPanel.gameObject.SetActive(true);

	}
}
