using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	void Awake()
	{
		GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<MenuMusic>().PlayMusic();
	}

	/* Buttons OnClick functions */
	public void NewGame()
	{
		PlayerPrefs.SetString("LastScene", "MainMenu");
		SceneManager.LoadScene("Game");
	}

	public void OpenCredits()
	{
		PlayerPrefs.SetString("LastScene", "MainMenu");
		SceneManager.LoadScene("Credits");
	}

	public void OpenOptions()
	{
		PlayerPrefs.SetString("LastScene", "MainMenu");
		SceneManager.LoadScene("Options");
	}

	public void Quit()
	{
		Application.Quit();
	}
}
