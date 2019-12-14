using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopBarTooltip : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void MoveTo(Vector3 destination)
    {
        transform.position = destination;
    }
}
