using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildButtonResourceDisplay : MonoBehaviour
{
    public Image m_image;
    public TextMeshProUGUI m_text;
    public void SetText(string text){
        m_text.text = text;
    }
    public void SetImage(Sprite sprite){
        m_image.sprite = sprite;
    }

}
