using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public InitObject[] objects;
    public CameraController cameraController;
    void Awake()
    {
        cameraController.enabled = false;
        foreach (InitObject item in objects)
        {
            item.Object.SetActive(item.Enabled);
        }
    }
}

[System.Serializable]
public class InitObject
{
    public GameObject Object;
    public bool Enabled;
}