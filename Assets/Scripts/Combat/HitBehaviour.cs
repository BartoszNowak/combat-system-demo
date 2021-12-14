using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBehaviour : StateMachineBehaviour, IAction
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		var actionManager =  animator.GetComponent<ActionManager>();
		actionManager.StartAction(this);
		animator.SetBool("takingDamage", true);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		var actionManager = animator.GetComponent<ActionManager>();
		actionManager.CancelCurrentAction();
		animator.GetComponent<Health>().GainIFrames(animator);
	}
}