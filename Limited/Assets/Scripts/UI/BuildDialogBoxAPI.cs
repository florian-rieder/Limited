using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildDialogBoxAPI : MonoBehaviour
{
	private Image image;
	private RectTransform rectTransform;
	public ButtonListControl btnListControl;

	public Tilemap environment;

	private bool btnListControlInitialized = false;

	void Awake()
	{
		// store references to other components of this gameobject
		image = GetComponent<Image>();
		rectTransform = (RectTransform)gameObject.transform;
	}
	public void MoveTo(Vector3Int destination)
	{
		// prevents the dialog box to display all possible buildings the first time it is enabled
		if (btnListControlInitialized == false)
		{
			EnvironmentTile tile;
			GameTiles.instance.environmentTiles.TryGetValue(destination, out tile);
			btnListControl.Initialize(tile);

			btnListControlInitialized = true;
		}

		// Offset dialog box so that its top left corner is in the middle of the clicked tile
		Vector3 adjustedDestination = new Vector3(destination.x + 0.5f, destination.y + 0.5f, transform.position.z);
		transform.position = adjustedDestination;
	}
	public bool IsOpen()
	{
		return gameObject.activeSelf;
	}
	public void Enabled(bool value)
	{
		gameObject.SetActive(value);
	}
	public Vector2 GetSize()
	{
		// create return object
		Vector2 size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
		return size;
	}

	public void UpdateButtons(Vector3Int tilePos)
	{
		// get currently clicked on tile
		EnvironmentTile tile;
		GameTiles.instance.environmentTiles.TryGetValue(tilePos, out tile);

		// update buttons
		btnListControl.UpdateButtons(tile);

	}
}
