using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class PlayerAttacker : Attacker
    {
        [SerializeField]
        private TargetIndicator targetIndicator;

		private void Update()
		{
            if (health.IsDead()) return;

            timeSinceLastAttack += Time.deltaTime;
            TargetEnemy();
        }

        private void TargetEnemy()
        {
            var hits = Physics.OverlapSphere(transform.position, currentWeapon.GetRange());

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
