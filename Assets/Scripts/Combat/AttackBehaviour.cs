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
            if (animator.gameObject.tag != "Player") return;

            var attacker = animator.GetComponent<Attacker>();
            var weapon = attacker.GetCurrentWeapon();
            animator.SetBool("hyperArmor", weapon.HasHyperArmor());

            var actionManager = animator.GetComponent<ActionManager>();
            actionManager.StartAction(this);
            //weapon.TrailEnabled(true);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("hyperArmor", false);
            animator.ResetTrigger("hit");
            if (animator.gameObject.tag != "Player") return;

            var actionManager = animator.GetComponent<ActionManager>();
            actionManager.CancelCurrentAction();
            animator.ResetTrigger("attack");

            var attacker = animator.GetComponent<Attacker>();
            var weapon = attacker.GetCurrentWeapon();
            //weapon.TrailEnabled(false);
        }
    }
}
