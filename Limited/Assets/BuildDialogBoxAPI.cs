using UnityEngine.UI;
using UnityEngine;

public class BuildDialogBoxAPI : MonoBehaviour
{
    public void MoveTo(Vector3 destination){
        transform.position = destination;
    }

    public bool isOpen(){
        var imageComponent = GetComponent<Image>();
        return imageComponent.enabled;
    }

    public Vector2 GetSize(){
        var imageComponent = GetComponent<Image>();
        Vector2 size = new Vector2(0, 0);
        return  size;
    }
}
