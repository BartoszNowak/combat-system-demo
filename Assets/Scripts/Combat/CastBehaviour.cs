using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class CastBehaviour : StateMachineBehaviour, IAction
    {
        private float animationTime = 0f;

        private float lengthTriggerTime;

        private bool start;
        private bool end;

        private AudioSource audioSource;

        void Enter(Animator animator)
        {
            var attacker = animator.GetComponent<Attacker>();
            var spell = attacker.CurrentSpell;
            animator.SetBool("hyperArmor", spell.HasHyperArmor);

            PlayAttackSound(animator, spell.AttackSound);
        }

        void Exit(Animator animator)
        {
            if(animator.GetBool("hyperArmor"))
			{
                animator.ResetTrigger("hit");
            }
            animator.SetBool("hyperArmor", false);
            animator.ResetTrigger("attack");

            var actionManager = animator.GetComponent<ActionManager>();
            actionManager.CancelCurrentAction();
        }

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
            var actionManager = animator.GetComponent<ActionManager>();
            actionManager.StartAction(this);
            lengthTriggerTime = stateInfo.length * 0.1f;
            start = false;
            end = false;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
            
            animationTime = 0f;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
            animationTime += Time.deltaTime;

            if(animationTime >= lengthTriggerTime && !start)
			{
                Enter(animator);
                start = true;
			}
            if (animationTime >= stateInfo.length - lengthTriggerTime && !end)
            {
                Exit(animator);
                end = true;
            }
        }

        private void PlayAttackSound(Animator animator, AudioClip attackSound)
		{
            audioSource = animator.GetComponent<AudioSource>();
            audioSource.clip = attackSound;
            audioSource.Play();
        }
	}
}
