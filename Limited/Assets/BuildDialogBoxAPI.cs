using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildDialogBoxAPI : MonoBehaviour
{
    private Image image;
    private RectTransform rectTransform;

    public Tilemap environment;

    void Awake(){
        image = GetComponent<Image>();
        rectTransform = (RectTransform)gameObject.transform;
    }
    public void MoveTo(Vector3Int destination){
        // Offset dialog box so that it's top left corner is in the middle of the clicked tile
        Vector3 adjustedDestination = new Vector3(destination.x + 0.5f, destination.y + 0.5f, transform.position.z);
        transform.position = adjustedDestination;
    }
    public bool IsOpen(){
        return gameObject.activeSelf;
    }
    public void Enabled(bool value){
        gameObject.SetActive(value);
    }
    public Vector2 GetSize(){
        // create return object
        Vector2 size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        return size;
    }
}
