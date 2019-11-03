using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
	public static TileSelector instance;
	public Tilemap environmentTilemap;

	public void MoveTo(Vector3Int position)
	{
		Vector3 newPos = new Vector3(position.x + 0.5f, position.y + 0.5f, position.z);
		transform.position = newPos;
	}
	public void Enabled(bool value)
	{
		gameObject.SetActive(value);
	}

	void Update()
	{
		Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3Int tilePos = environmentTilemap.WorldToCell(point);
        Vector3 newPos = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f, tilePos.z);

        if(transform.position != newPos){
            transform.position = newPos;
        }

	}
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}
}
