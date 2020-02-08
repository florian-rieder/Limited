using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBox : MonoBehaviour
{
	public GameObject gameController;
	public CameraController cameraController;
	public Animator animator;
	public TutorialController tutorialController;

	[SerializeField] private GameObject[] HUDElementsToShow;

	// Update is called once per frame
	void Update()
	{
		// close info box on first click since it has been awaken
		if (Input.GetMouseButtonDown(0))
		{
			// start game by activating the gamecontroller
			gameController.SetActive(true);
			// allow camera movement
			cameraController.enabled = true;

			// Show HUD elements
			foreach (var HUDElement in HUDElementsToShow)
			{
				HUDElement.SetActive(true);
			}

			// trigger closing animation
			animator.SetTrigger("CloseTrigger");
		}
	}

	public void OnExitAnimationEnd()
	{
		// start tutorial
		tutorialController.Initialize();

		// disable this script
		gameObject.SetActive(false);
	}
}
