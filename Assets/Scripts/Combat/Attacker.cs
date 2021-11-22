using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField]
        private Transform rightHand = null;
        [SerializeField]
        private Transform leftHand = null;
        [SerializeField]
        private Weapon defaultWeapon = null;

        private Animator animator;
        private Mover movement;
        private ActionManager actionManager;
		private Health health;

        private Transform target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeapon = null;

        private void Start()
        {
            animator = GetComponent<Animator>();
            actionManager = GetComponent<ActionManager>();
            movement = GetComponent<Mover>();
            health = GetComponent<Health>();
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            if (health.IsDead()) return;

            timeSinceLastAttack += Time.deltaTime;

            TargetEnemy();
        }

        public void Attack()
        {
            if (timeSinceLastAttack < currentWeapon.GetTimeBetweenAttacks()) return;
            if (target != null)
            {
                transform.LookAt(target);
            }
            TriggerAttack();
            timeSinceLastAttack = 0f;
        }
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            currentWeapon.Equip(rightHand, leftHand, animator);
        }

        public Weapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public void Cancel()
        {
            target = null;
            timeSinceLastAttack = Mathf.Infinity;
        }

        private void TriggerAttack()
        {
            //Triggers Hit and Shoot methods
            animator.SetTrigger("attack");
            animator.ResetTrigger("attackCancel");
        }

        private void TriggerAttackCancel()
        {
            //Triggers Hit and Shoot methods
            animator.ResetTrigger("attack");
            animator.SetTrigger("attackCancel");
        }

        //Animation trigger
        void Hit()
        {
			if (target == null) return;

			var health = target.GetComponent<Health>();            
			if (health != null)
			{
                Debug.Log($"health component = {health.name}, damage = {currentWeapon.GetDamage()}, attacker = {gameObject.name}");
                health.DealDamage(currentWeapon.GetDamage(), gameObject);
			}
            if(currentWeapon.CanCancelAnimation())
			{
                actionManager.CancelCurrentAction();
                TriggerAttackCancel();
            }
		}

        //Animation trigger
        void Shoot()
        {
            //if (target == null)
            //{
            //    currentWeapon.LaunchProjectile(rightHand, leftHand, transform.forward, gameObject);
            //}
            //else
            //{
            //    var targetHealth = target.GetComponent<Health>();
            //    currentWeapon.LaunchProjectile(rightHand, leftHand, targetHealth, gameObject);
            //}
        }

        private void TargetEnemy()
        {
            var hits = Physics.OverlapSphere(transform.position, currentWeapon.GetRange());

            var shortestRange = Mathf.Infinity;
            var smallestAngle = Mathf.Infinity;
            Transform targetEnemy = null;

            foreach (var hit in hits)
            {
                if (hit.transform.tag != "Enemy") continue;
                

                var directionVector = hit.transform.position - transform.position;
                var distance = directionVector.magnitude;
                var angle = Vector3.Angle(directionVector, transform.forward);

                //if (distance < shortestRange && angle <= 80f)
                if (angle <= 70f && distance < currentWeapon.GetRange() && angle < smallestAngle)
                {
                    shortestRange = distance;
                    targetEnemy = hit.transform;
                }
            }
            if (target != null && target != targetEnemy)
            {
                target.GetComponent<Target>().RemoveTarget();
                target = null;
            }
            if (targetEnemy != null)
            {
                target = targetEnemy;
                target.GetComponent<Target>().SetAsTarget();
            }
        }
    }
}

