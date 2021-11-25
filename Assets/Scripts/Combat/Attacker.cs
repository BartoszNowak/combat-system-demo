using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Attacker : MonoBehaviour
    {
		private const float InteractionAngle = 70f;

		[SerializeField]
        private Transform rightHand = null;
        [SerializeField]
        private Transform leftHand = null;
        [SerializeField]
        internal Weapon defaultWeapon = null;

        internal Animator animator;
        internal ActionManager actionManager;
		internal Health health;
        internal Mover mover;

        internal Transform target;
        internal float timeSinceLastAttack = Mathf.Infinity;
        internal Weapon currentWeapon = null;

        private void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            actionManager = GetComponent<ActionManager>();
            health = GetComponent<Health>();
            EquipWeapon(defaultWeapon);
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

        internal void TriggerAttack()
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
            Camera.main.GetComponent<CameraShake>().TriggerShake(currentWeapon.GetFeedbackShakePower());
            if (target == null) return;
            if (!CanAttack(target)) return;

			var health = target.GetComponent<Health>();            
			if (health != null)
			{
                target.GetComponent<ActionManager>().CancelCurrentAction();
                target.GetComponent<Mover>().Knockback(transform.forward, currentWeapon.GetKnockback());
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

		internal bool CanAttack(float angle, float distance)
		{
			return angle <= InteractionAngle && distance < currentWeapon.GetRange();
		}

		internal bool CanAttack(Transform targetEnemy)
        {
            var directionVector = targetEnemy.position - transform.position;
            var distance = directionVector.magnitude;
            var angle = Vector3.Angle(directionVector, transform.forward);
            return angle <= InteractionAngle && distance < currentWeapon.GetRange();
        }
    }
}

