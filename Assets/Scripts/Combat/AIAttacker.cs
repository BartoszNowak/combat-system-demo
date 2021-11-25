using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(ActionManager))]
    public class AIAttacker : Attacker
    {

		private void Update()
        {
            if (target == null) return;

            if (target.GetComponent<Health>().IsDead() || health.IsDead())
            {
                Cancel();
                return;
            }

            timeSinceLastAttack += Time.deltaTime;

			if (!IsInRange() && !actionManager.HasActiveAction())
			{
				mover.MoveTo(target.position);
			}
			else
			{
				HandleAttack();
			}
		}

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < currentWeapon.GetRange();
        }

        private void HandleAttack()
        {
            mover.Stop();
            if (timeSinceLastAttack >= currentWeapon.GetTimeBetweenAttacks() && IsInRange())
            {
                transform.LookAt(target);
                TriggerAttack();
                timeSinceLastAttack = 0f;
                actionManager.CancelCurrentAction();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            var targetHealth = combatTarget.GetComponent<Health>();
            return !targetHealth.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            target = combatTarget.transform;
        }
    }
}