using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
	public static TileSelector instance;
	public Tilemap environmentTilemap;
	private bool locked = false;
	private Vector3 lastPosition;

	public void Enabled(bool value)
	{
		gameObject.SetActive(value);
	}

	public void LockPosition(bool value)
	{
		locked = value;
	}

	void Start()
	{
		lastPosition = transform.position;
	}

	void Update()
	{
		Vector3 newPos = lastPosition;

		if (!locked)
		{
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3Int tilePos = environmentTilemap.WorldToCell(point);
			newPos = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f, tilePos.z);
		}

		if (transform.position != newPos)
		{
			transform.position = newPos;
			lastPosition = newPos;
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
