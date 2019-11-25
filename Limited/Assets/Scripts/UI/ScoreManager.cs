using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Doing this in awake permits the score to be computed when the GameOverPanel is activated
    // therefore, the score is computed on the moment the player loses.
    void Awake(){
        // get text component
        TextMeshProUGUI totalScore = GetComponent<TextMeshProUGUI>();
        // change text displayed to the score
        totalScore.text = "Score: " + GetScore();
    }
	private int GetScore()
	{
		int score = 0;

		Dictionary<Vector3Int, FacilityTile> facilities = GameTiles.instance.facilitiesTiles;

        // Add points to the score according to different metrics

        // get points for each facility built
		foreach (KeyValuePair<Vector3Int, FacilityTile> entry in facilities)
		{
            FacilityTile fTile = entry.Value;
            score += fTile.ScorePoints;
		}

		return score;
	}
}
