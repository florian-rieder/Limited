using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public static GameController instance;
	public Color negativeColor;
	public Color positiveColor;
	public float famineTimerBase = 30f; // duration of famine in [s] until the game is lost
	public PlayerInventory playerInventory;
	public BigNotificationAPI notificationBig;
	public TilemapInteraction tilemapInteraction;
	public GameOverPanel gameOverPanel;
	public FamineTimerDisplay famineTimerDisplay;
	public CameraController cameraController;
	public BuildDialogBoxAPI buildDialog;
	public GrowthBar growthBar;
	public TopBar topBar;
	public AudioManager audioManager;

	// time in [s] that has elapsed
	private float timer = 0f;
	private float famineTimer = 0f;
	// controls if we count the time elapsed
	private bool updateTimer = false;
	private bool updateFamineTimer = false;
	private float nextGrowthTime = 0f;

	private bool firstCityTutorialEnabled = true;
	private bool firstCityBuilt = false;
	private bool gameOver = false;

	private int pollutionRenewCounter = 0;

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

		if (!firstCityBuilt && GameTiles.instance.GetCities().Count == 1)
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
				NewCity();
				firstCityBuilt = true;
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
			UpdateGrowthBar();
		}

		bool hasCityNeeds = playerInventory.hasCityNeeds();

		if (firstCityBuilt)
		{
			// if needs aren't met and famine timer is not running
			if (!hasCityNeeds && !updateFamineTimer)
			{
				// start the famine
				EnableFamineTimer(true);
			}

			// if the needs are met and the famine timer is running
			if (hasCityNeeds && updateFamineTimer)
			{
				// stop the famine
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
		InvokeRepeating("ProduceResources", 1, 1);
		InvokeRepeating("RenewResources", 1, 1);
	}

	public void NewCity()
	{
		// if a dialog box is open, close it
		if (buildDialog.IsOpen())
		{
			buildDialog.Enabled(false);
		}

		// Get all possible tiles to found a city
		var possibleLocations = GameTiles.instance.GetPossibleCityTiles();

		if (possibleLocations.Count == 0) GameOver("Your city ran out of space to expand on !");

		// highlight them in green
		foreach (EnvironmentTile eTile in possibleLocations)
		{
			tilemapInteraction.HighlightTile(eTile);
		}

		// move the camera to the center of the city (but only if there is a city)
		if (GameTiles.instance.GetCities().Count != 0)
		{
			cameraController.MoveTo(GetCityCenter());
			audioManager.Play("new_city");
		}
	}

	private Vector3 GetCityCenter()
	{
		Vector3 cityCenter = Vector3.zero;
		var cities = GameTiles.instance.GetCities();

		foreach (KeyValuePair<Vector3Int, FacilityTile> entry in cities)
		{
			Vector3Int position = entry.Key;

			cityCenter += position;
		}

		cityCenter.x = cityCenter.x / cities.Count;
		cityCenter.y = cityCenter.y / cities.Count;

		return cityCenter;

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
		float baseTime = 70f; // time in [s]
		float mildness = 10f;
		int cities = GameTiles.instance.GetCities().Count;

		float time = 0f;

		// get the time in [s] before the next expansion of the city
		// in function of the number of cities owned and our parameters
		time = baseTime / (1 + (cities / mildness));

		return time;
	}

	private void ProduceResources()
	{
		var facilities = GameTiles.instance.facilitiesTiles;
		bool stopCheckingProduction = false;

		// for each facility
		foreach (KeyValuePair<Vector3Int, FacilityTile> entry in facilities)
		{
			var facility = entry.Value;
			if (facility.Extractor) facility.Extract();
			else if (facility.Name != "City" && !stopCheckingProduction) stopCheckingProduction = facility.Produce();
		}
	}
	private void RenewResources()
	{
		var environmentTiles = GameTiles.instance.environmentTiles;
		foreach (KeyValuePair<Vector3Int, EnvironmentTile> entry in environmentTiles)
		{
			var tile = entry.Value;
			if (tile.Name == "Forest")
			{
				if (tile.Polluted && pollutionRenewCounter == 2)
				{
					tile.Resources["Wood"] += 1;
					pollutionRenewCounter = 0;
				}
				if (!tile.Polluted)
				{
					tile.Resources["Wood"] += 1;
				}
			}
		}

		pollutionRenewCounter++;
	}
	public void EnableTimer(bool value)
	{
		growthBar.Enable(value);
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
	public void UpdateGrowthBar()
	{
		float ratio = timer / nextGrowthTime;
		growthBar.SetValue(ratio);

	}
	public void GameOver(string reason = "No reason specified.")
	{
		if (!gameOver)
		{
			Debug.Log("Game Over.");

			// hide some UI elements
			TileSelector.instance.Enabled(false);
			growthBar.Enable(false);
			topBar.Enable(false);

			// open game over panel
			gameOverPanel.SetReason(reason);
			gameOverPanel.gameObject.SetActive(true);

			cameraController.TriggerShake(1f, 0.2f);
			gameOver = true;
		}
	}
}
