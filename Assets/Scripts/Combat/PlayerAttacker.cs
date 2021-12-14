using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class PlayerAttacker : Attacker
    {
        [SerializeField]
        private TargetIndicator targetIndicator;

        public event Action OnManaWarning;

        public event Action OnFailSwordOnly;
        public event Action OnFailMagicOnly;

        private void Update()
		{
            if (health.IsDead()) return;

            timeSinceLastAttack += Time.deltaTime;
            timeSinceLastSpell += Time.deltaTime;
            TargetEnemy();
        }

		private void OnEnable()
		{
            actionManager.OnStateChange += HitEnd;
		}

		void HitStart()
        {
            currentWeapon.EnableHitBox(true);
        }

        void HitEnd()
        {
            currentWeapon.EnableHitBox(false);
        }

        public void FailSwordOnly()
		{
            OnFailSwordOnly?.Invoke();
		}

        public void FailMagicOnly()
        {
            OnFailMagicOnly?.Invoke();
        }

        public override void CastSpell(Vector3 direction)
        {
            if (actionManager.HasActiveAction()) return;
            if (timeSinceLastSpell < currentSpell.TimeBetweenCasts) return;

            if (mana != null && !mana.CanCast(currentSpell.ManaCost))
            {
                OnManaWarning?.Invoke();
                return;
            }
            else
            {
                mana.DecreaseMana(currentSpell.ManaCost);
            }
            if (target != null)
            {
                transform.LookAt(target);
            }
            else if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            TriggerSpellCast();
            timeSinceLastSpell = 0f;
        }

        private void TargetEnemy()
        {
            var range = Mathf.Max(currentWeapon.GetRange(), currentSpell.Range);
            var hits = Physics.OverlapSphere(transform.position, range);

            var smallestAngle = Mathf.Infinity;
            Transform targetEnemy = null;

            foreach (var hit in hits)
            {
                if (hit.transform.tag != "Enemy") continue;

                var directionVector = hit.transform.position - transform.position;
                var distance = directionVector.magnitude;
                var angle = Vector3.Angle(directionVector, transform.forward);

                if (CanAttack(angle, distance) && angle < smallestAngle)
                {
                    smallestAngle = angle;
                    targetEnemy = hit.transform;
                }
            }
            if (target != null && target != targetEnemy)
            {
                targetIndicator.RemoveTarget();
                target = null;
            }
            if (targetEnemy != null)
            {
                target = targetEnemy;
                targetIndicator.SetAsTarget(targetEnemy);
            }
        }
    }
}
