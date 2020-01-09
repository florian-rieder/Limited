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
	public FamineDisplay famineDisplay;
	public CameraController cameraController;
	public BuildDialogBoxAPI buildDialog;
	public UIBar growthBar;
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
		// Singleton
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		StartGame();
	}

	void Update()
	{
		// tutorial logic
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

		// tutorial logic
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
		// if a build dialog box is open, close it
		if (buildDialog.IsOpen())
		{
			buildDialog.Enabled(false);
		}

		// Get all possible tiles to found a city
		var possibleLocations = GameTiles.instance.GetPossibleCityTiles();

		// Loose if there is no space to expand the city on
		if (possibleLocations.Count == 0)
		{
			GameOver("Your city ran out of space to expand on !");
			return;
		}

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
		/* Get the center of the city based on all the city tiles positions */

		Vector3 cityCenter = Vector3.zero;
		var cities = GameTiles.instance.GetCities();

		// compute mean position of all cities
		foreach (KeyValuePair<Vector3Int, FacilityTile> entry in cities)
		{
			// Add all position vectors
			cityCenter += entry.Key;
		}
		// divide the position components by the total amount of cities
		cityCenter.x = cityCenter.x / cities.Count;
		cityCenter.y = cityCenter.y / cities.Count;

		// return the mean position
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
		float baseTime = 40f; // time in [s]
		float mildness = 50f;
		int cities = GameTiles.instance.GetCities().Count;
		float difficulty = PlayerPrefs.GetFloat("difficulty");

		// apply difficulty
		float newBaseTime = baseTime / 2 + baseTime * (1 - difficulty);
		float newMildness = mildness / 4 + mildness * (1 - difficulty);

		float time = 0f;

		// get the time in [s] before the next expansion of the city
		// in function of the number of cities owned and our parameters
		time = newBaseTime / (1 + (cities / newMildness));

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

			// don't check facilities if all the resources are in the green, except for the facilities that are already stopped,
			// for which we need to check if they can be enabled again
			else if (facility.Name != "City" && (!stopCheckingProduction || !facility.IsWorking)) stopCheckingProduction = facility.Produce();
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
				// Forests grow, but this growth is impacted by pollution
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
		famineDisplay.Enable(value);
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
		// if the game is already over, ignore this call
		if (gameOver) return;

		// hide some UI elements
		TileSelector.instance.Enabled(false);
		growthBar.Enable(false);
		famineDisplay.gameObject.SetActive(false);
		topBar.Enable(false);

		// open game over panel
		gameOverPanel.SetReason(reason);
		gameOverPanel.gameObject.SetActive(true);

		// Shake the camera
		cameraController.TriggerShake(1f, 0.2f);

		// play game over sound
		audioManager.Play("game_over");

		// Wait until the end of the screenshake to pause the time
		// otherwise, the screen keeps shaking forever
		Invoke("PauseTime", 0.5f);

		gameOver = true;

		Debug.Log("Game Over.");
	}
	public void pauseTime()
	{
		Time.timeScale = 0f;
		// disable camera controls
		cameraController.enabled = false;
	}
}
