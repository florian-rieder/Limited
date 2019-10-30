using UnityEngine;

public class ButtonListControl : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;

    void Start(){
        for (int i = 1; i <= 10; i++)
        {
            // instantiate new button
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            // change text of the button
            button.GetComponent<BuildButton>().SetText("button #" + i);

            // set parent of the button with the parent of the button template's parent 
            // (our content object)
            button.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }
}
