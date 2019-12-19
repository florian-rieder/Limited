using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject topBarUI;
	public GameObject tileSelector;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	public void Resume()
	{
		tileSelector.SetActive(true);
        topBarUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
	}

	void Pause()
	{
		tileSelector.SetActive(false);
        topBarUI.SetActive(false);
        pauseMenuUI.SetActive(true);

        // freezes the game
        Time.timeScale = 0f;

        GameIsPaused = true;
	}

    public void LoadMenu(){
        Time.timeScale = 1f;
		PlayerPrefs.SetString("LastScene", "Game");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame(){
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
