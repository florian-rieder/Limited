using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TilemapInteraction : MonoBehaviour
{
	public Tilemap environmentTilemap;
	public Tilemap facilitiesTilemap;
	public BuildDialogBoxAPI dialogBox;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			// prevents clicking through UI
			if (EventSystem.current.IsPointerOverGameObject())
			{
				Debug.Log("Clicked on the UI");
			}
			else
			{
				EnvironmentTile _environmentTile;
				FacilityTile _facilityTile;

				// get the position of the tile being clicked on
				Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				var tilePos = environmentTilemap.WorldToCell(point);

				doDialogBox(tilePos);

				var environmentTiles = GameTiles.instance.environmentTiles; // This is our Dictionary of tiles
				var facilitiesTiles = GameTiles.instance.facilitiesTiles;


				// if we have a tile registered at this position
				if (environmentTiles.TryGetValue(tilePos, out _environmentTile))
				{
					// Do something
					// Debug
					Debug.Log(createEnvironmentLog(_environmentTile));

					// turn tile green
					_environmentTile.TilemapMember.SetTileFlags(_environmentTile.LocalPlace, TileFlags.None);
					_environmentTile.TilemapMember.SetColor(_environmentTile.LocalPlace, Color.green);
				}

				if (facilitiesTiles.TryGetValue(tilePos, out _facilityTile))
				{

					// debug
					Debug.Log(createFacilityLog(_facilityTile));
				}
			}

		}
	}

	private string createFacilityLog(FacilityTile ft)
	{
		string str = "";
		str += "name: " + ft.Name;
		str += "\nSprite: " + ft.TileBase.ToString();
		str += "\nOil: " + ft.Oil;
		str += "\nCoal: " + ft.Coal;
		str += "\nWood: " + ft.Wood;
		str += "\nPower: " + ft.Power;
		str += "\nGoods: " + ft.Goods;
		str += "\nFood: " + ft.Food;
		str += "\nPollution radius: " + ft.PollutionRadius;

		return str;
	}

	private string createEnvironmentLog(EnvironmentTile et)
	{
		string str = "";
		str += "name: " + et.Name;
		str += "\nSprite: " + et.TileBase.ToString();
		str += "\nOil: " + et.Oil;
		str += "\nCoal: " + et.Coal;
		str += "\nWood: " + et.Wood;

		return str;
	}

	private void doDialogBox(Vector3Int pos){
		if(dialogBox.IsOpen()){
			dialogBox.Enabled(false);
		} else {
			dialogBox.MoveTo(pos);
			dialogBox.Enabled(true);
		}
	}

}
