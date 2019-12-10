using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBox : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
        // close info box on first click since it has been awaken
		if (Input.GetMouseButtonDown(0))
		{
            gameObject.SetActive(false);
		}
	}
}
