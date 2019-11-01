using UnityEngine;
using TMPro; // Text Mesh Pro namespace

public class BuildButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_text; // the text component of our button
    [SerializeField]
    private ButtonListControl btnControl;
    private string m_textString;
    public void SetText(string textString){
        // we store the text string and assign it to our text component
        m_textString = textString;
        m_text.text = m_textString;
    }
    public void OnClick(){
        btnControl.ButtonClicked(m_textString);
    }
}
