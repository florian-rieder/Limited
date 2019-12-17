using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public InitObject[] objects;
    public CameraController cameraController;
    public GameTiles gameTiles;
    public TileAutomata mapGenerator;
    void Awake()
    {
        cameraController.enabled = false;

        // generate map
        mapGenerator.doSim(30);
        gameTiles.GetWorldTiles();

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