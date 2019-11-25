using UnityEngine;
using System.Collections.Generic;

public class TopBar : MonoBehaviour
{
	/* Interface to manage the top UI bar of the game.
     * Generates a display for each resource there is in the game and updates
     * them according to the facilities on the map
     */

	[SerializeField]
	private PlayerInventory playerInventory;
	[SerializeField]
	private GameObject itemTemplate;

	[SerializeField]
	private Sprite oilIcon;
	[SerializeField]
	private Sprite coalIcon;
	[SerializeField]
	private Sprite woodIcon;
	[SerializeField]
	private Sprite metalIcon;
	[SerializeField]
	private Sprite powerIcon;
	[SerializeField]
	private Sprite goodsIcon;
	[SerializeField]
	private Sprite foodIcon;

	private List<TopBarResourceDisplay> displays = new List<TopBarResourceDisplay>();
	private Dictionary<string, Sprite> resources = new Dictionary<string, Sprite>();

	void Start()
	{
		// generate list of resources
		resources.Add("Oil", oilIcon);
		resources.Add("Coal", coalIcon);
		resources.Add("Wood", woodIcon);
		resources.Add("Metal", metalIcon);
		resources.Add("Power", powerIcon);
		resources.Add("Goods", goodsIcon);
		resources.Add("Food", foodIcon);

		// generate displays
		foreach (KeyValuePair<string, Sprite> entry in resources)
		{
			Sprite icon = entry.Value;
			string name = entry.Key;

			// create new display
			GameObject item = Instantiate(itemTemplate);
			item.SetActive(true);

			var display = item.GetComponent<TopBarResourceDisplay>();

			display.SetIcon(icon);
			display.SetName(name);

			displays.Add(display);

			// set parent of the button with the parent of the button template's parent 
			// (our content object)
			item.transform.SetParent(itemTemplate.transform.parent, false);
		}
	}

	void Update()
	{
		// get current inventory values
		Dictionary<string, int> values = playerInventory.getCount();

		// Update old values for each display
		foreach (TopBarResourceDisplay display in displays)
		{
			display.SetValue(values[display.GetName()]);
		}
	}

	public void Enable(bool value)
	{
		gameObject.SetActive(value);
	}
}