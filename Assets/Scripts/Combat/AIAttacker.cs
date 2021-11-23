using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(AIMover))]
    [RequireComponent(typeof(ActionManager))]
    public class AIAttacker : MonoBehaviour, IAction
    {
        [SerializeField]
        private Transform rightHand = null;
        [SerializeField]
        private Transform leftHand = null;
        [SerializeField]
        private Weapon defaultWeapon = null;

        private Animator animator;
        private AIMover movement;
        private ActionManager actionManager;

        private Transform target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeapon = null;

        private void Start()
        {
            animator = GetComponent<Animator>();
            actionManager = GetComponent<ActionManager>();
            movement = GetComponent<AIMover>();
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            if (target == null) return;

            if (target.GetComponent<Health>().IsDead() || GetComponent<Health>().IsDead())
            {
                Cancel();
                return;
            }

            timeSinceLastAttack += Time.deltaTime;
            if (!IsInRange())
            {
                movement.MoveTo(target.position);
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

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            currentWeapon.Equip(rightHand, leftHand, animator);
        }

        private void HandleAttack()
        {
            movement.Stop();
            if (timeSinceLastAttack >= currentWeapon.GetTimeBetweenAttacks())
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
            actionManager.StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
            timeSinceLastAttack = Mathf.Infinity;
            TriggerAttackCancel();
        }

        private void TriggerAttack()
        {
            animator.SetTrigger("attack");
        }

        private void TriggerAttackCancel()
        {
            animator.ResetTrigger("attack");
        }

        //Animation trigger
        void Hit()
        {
            if (target == null) return;
            if (!IsInRange()) return;

            var health = target.GetComponent<Health>();
            if (health != null)
            {
                health.DealDamage(currentWeapon.GetDamage(), gameObject);
            }
        }

        void Shoot()
        {
            if (target == null) return;

            var health = target.GetComponent<Health>();
            if (health == null) return;

            //currentWeapon.LaunchProjectile(rightHand, leftHand, health, gameObject);
        }
    }
}