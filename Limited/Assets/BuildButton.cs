using UnityEngine;
using TMPro; // Text Mesh Pro namespace

public class BuildButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text; // the text component of our button
    public void SetText(string textString){
        text.text = textString;
    }
    public void OnClick(){

    }
}
