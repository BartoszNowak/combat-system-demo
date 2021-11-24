using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AttackBehaviour : StateMachineBehaviour, IAction
    {
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var attacker = animator.GetComponent<Attacker>();
            var weapon = attacker.GetCurrentWeapon();
            animator.SetBool("hyperArmor", weapon.HasHyperArmor());

            var actionManager = animator.GetComponent<ActionManager>();
            actionManager.StartAction(this);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(animator.GetBool("hyperArmor"))
			{
                animator.ResetTrigger("hit");
            }
            animator.SetBool("hyperArmor", false);

            var actionManager = animator.GetComponent<ActionManager>();
            actionManager.CancelCurrentAction();
            animator.ResetTrigger("attack");
        }
    }
}
