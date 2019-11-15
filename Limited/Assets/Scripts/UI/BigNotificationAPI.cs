using System.Collections;
using System; // Action
using UnityEngine.UI; // Text component interaction
using UnityEngine;

public class BigNotificationAPI : MonoBehaviour
{
    public float fadeOutTime = 10f;
    public Color colorOverride;

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine(resetText));
    }

    public void SetText(string text){
        var textComponent = GetComponent<Text>();
        textComponent.enabled = true;
        textComponent.text = text;
    }

    private IEnumerator FadeOutRoutine(Action endCallback)
    {
        Text text = GetComponent<Text>();
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            // fade out the alpha value
            text.color = new Color(text.color[0], text.color[1], text.color[2], Mathf.Lerp(1f, 0f, t / fadeOutTime));
            yield return null;
        }

        if(endCallback != null) endCallback();
    }

    private void resetText(){
        var text = GetComponent<Text>();
        text.enabled = false;
        text.color = colorOverride;
    }
}
