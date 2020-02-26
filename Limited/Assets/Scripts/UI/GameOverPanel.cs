using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI reasonText;
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private TextMeshProUGUI highscoreMarker;

	public void SetReason(string text)
	{
		reasonText.text = text;
	}

	public void SetScore(int score)
	{
		scoreText.text = score.ToString();
	}

	public void HighScore()
	{
		// just show the marker
		highscoreMarker.gameObject.SetActive(true);
	}

	public void BackToMainMenu()
	{
		// go to main menu
		PlayerPrefs.SetString("LastScene", "Game");
		SceneManager.LoadScene("MainMenu");
	}

	public void Retry()
	{
		// reload the game
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
