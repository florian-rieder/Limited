using UnityEngine;

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
	private float nextGrowthTime = 1f;

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

		if(timer >= nextGrowthTime){
			EnableTimer(false);
			ResetTimer();
			NewCity();
		}

		// count the time that has elapsed
		if (updateTimer)
		{
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

	public void CityBuilt(){
		/* triggers when the first city is built on the map */
		nextGrowthTime = GetNextCityGrowthTime();
		EnableTimer(true);
	}

	private float GetNextCityGrowthTime(){
		// parameters
		float maxTime = 15f;
		float steepness = 9f;

		float time = 0f;

		int cities = GameTiles.instance.GetCities().Count;

		time = maxTime / (1 + cities/steepness);

		return time;
	}

	public void EnableTimer(bool value){
		timerDisplay.Enable(value);
		updateTimer = value;
	}
	
	public void ResetTimer(){
		timer = 0f;
	}

	public float GetTimeRemaining(){
		return nextGrowthTime - timer;
	}
}


