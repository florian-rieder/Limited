using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildTooltip : MonoBehaviour
{
	public TextMeshProUGUI title;
	public TextMeshProUGUI description;
	public RectTransform transfo;
	public Texture2D resourcesSpritesheet;
	public GameObject resourceDisplayTemplate;
	public PlayerInventory playerInventory;

	private Dictionary<string, BuildButtonResourceDisplay> resourceDisplays;

	// don't scale with zoom
	private float orthoOrg;
	private float orthoCurr;
	private Vector3 scaleOrg;
	private Vector3 posOrg;

	void Awake()
	{
		GenerateResourcesDisplays();

		orthoOrg = Camera.main.orthographicSize;
		orthoCurr = orthoOrg;
		scaleOrg = transform.localScale;
		posOrg = Camera.main.WorldToViewportPoint(transform.position);
	}

	void Update()
	{
		// always stay the same size
		var osize = Camera.main.orthographicSize;
		if (orthoCurr != osize)
		{
			transform.localScale = scaleOrg * osize / orthoOrg;
			orthoCurr = osize;
		}
	}


	public void SetTitle(string titleString)
	{
		title.text = titleString;
	}

	public void SetDescription(string descriptionString)
	{
		description.text = descriptionString;
	}

	public void MoveTo(Vector3 position)
	{
		transfo.localPosition = position;
	}

	public void Enable(bool value)
	{
		gameObject.SetActive(value);
	}

	public void SetResourceDisplay(Dictionary<string, int> resources)
	{
		foreach (KeyValuePair<string, int> entry in resources)
		{
			string name = entry.Key;
			int value = entry.Value;

			//Debug.Log("set: " + name + ": " + value);

			var display = resourceDisplays[name];

			// don't show resources with a value of 0 (resources not used in the facility)
			if (value == 0)
			{
				display.Enable(false);
			}
			else
			{
				display.Enable(true);
				display.SetValue(value);
			}
		}
	}

	private void GenerateResourcesDisplays()
	{
		var resources = playerInventory.getCount();
		resourceDisplays = new Dictionary<string, BuildButtonResourceDisplay>();

		// generate displays
		foreach (KeyValuePair<string, int> entry in resources)
		{
			string name = entry.Key;
			int value = entry.Value;
			//Debug.Log("gen: " + name + ": " + value);

			// get sprite
			var names = new Dictionary<string, int>{
				{"Oil", 0},
				{"Coal", 1},
				{"Wood", 2},
				{"Metal", 6},
				{"Power", 3},
				{"Goods", 4},
				{"Food", 5}
			};

			// get all sliced tiles from our tileset
			Sprite[] resourceSprites = Resources.LoadAll<Sprite>(resourcesSpritesheet.name);
			Sprite sprite = resourceSprites[names[name]];

			// apply sprite and value to item
			GameObject display = Instantiate(resourceDisplayTemplate);
			BuildButtonResourceDisplay displayScript = display.GetComponent<BuildButtonResourceDisplay>();

			// initialize this resource display
			displayScript.Enable(false);
			displayScript.SetImage(sprite);
			displayScript.SetValue(value);

			// place display in the hierarachy
			display.transform.SetParent(resourceDisplayTemplate.transform.parent);

			// keep a reference to the display
			resourceDisplays.Add(name, displayScript);
		}
	}
}
