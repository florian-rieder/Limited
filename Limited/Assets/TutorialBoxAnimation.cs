using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoxAnimation : StateMachineBehaviour
{
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Debug.Log("OnStateExit TutorialBoxAnimation");
		// close the object this animation is attached to
		animator.gameObject.GetComponent<TutorialBox>().OnExitAnimationEnd();
	}

}