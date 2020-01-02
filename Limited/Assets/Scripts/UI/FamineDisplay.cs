using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FamineDisplay : MonoBehaviour
{
	public UIBar bar;
	public Image icon;
	public List<ColorStage> colorStages;
	private int currentStage;

    void Start()
    {
        bar.Enable(false);
    }

	void Update()
	{
		float currentTimeRatio = GameController.instance.GetFamineTimeRemaining() / GameController.instance.famineTimerBase;

		if (currentTimeRatio < colorStages[currentStage].threshold && currentStage + 1 < colorStages.Count)
		{
			currentStage++;
			UpdateColorStage();
		}

		// update the bar
		bar.SetValue(currentTimeRatio);
	}

	public void Enable(bool value)
	{
		if (!value)
		{
			// reset current stage
			currentStage = 0;
			UpdateColorStage();
		}

		bar.gameObject.SetActive(value);
	}

	private void UpdateColorStage()
	{
		// update color and icon of the famine display
		bar.bar.GetComponent<Image>().color = colorStages[currentStage].color;
		icon.sprite = colorStages[currentStage].icon;
	}
}

[System.Serializable]
public class ColorStage
{
	public Sprite icon;
	public Color color;
	[Range(0, 1)]
	public float threshold;
}