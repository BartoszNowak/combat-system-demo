using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class Attacker : MonoBehaviour
    {
		private const float InteractionAngle = 70f;

		[SerializeField]
        private Transform rightHand = null;
        [SerializeField]
        private Transform leftHand = null;
        [SerializeField]
        internal Weapon defaultWeapon = null;
        [SerializeField]
        internal Spell defaultSpell = null;

        internal Animator animator;
        internal ActionManager actionManager;
		internal Health health;
        internal Mana mana;
        internal Mover mover;

        internal Transform target;

        internal Weapon currentWeapon = null;
        internal float timeSinceLastAttack = Mathf.Infinity;

        internal Spell currentSpell = null;
        internal float timeSinceLastSpell = Mathf.Infinity;

        bool casting = false;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            actionManager = GetComponent<ActionManager>();
            health = GetComponent<Health>();
            mana = GetComponent<Mana>();
            EquipWeapon(defaultWeapon);
            AttuneSpell(defaultSpell);
            
        }

        private void OnDisable()
        {
            if (currentWeapon.HitBox == null) return;

            currentWeapon.HitBox.OnHit -= HitListener;
        }

		public void Attack()
        {
            if (actionManager.HasActiveAction()) return;
            if (timeSinceLastAttack < currentWeapon.GetTimeBetweenAttacks()) return;
            if (target != null)
            {
                transform.LookAt(target);
            }
            TriggerAttack();
            timeSinceLastAttack = 0f;
        }

        public abstract void CastSpell(Vector3 direction);

        public void EquipWeapon(Weapon weapon)
        {
            //currentWeapon.HitBox.OnHit -= HitListener;
            currentWeapon = weapon;
            currentWeapon.Equip(rightHand, leftHand, animator);

            if (currentWeapon.HitBox == null) return;
            currentWeapon.HitBox.OnHit += HitListener;
        }

        public void AttuneSpell(Spell spell)
        {
            if (spell == null) return;
            currentSpell = spell;
            currentSpell.Attune(animator);
        }

        public Weapon CurrentWeapon { get { return currentWeapon; } }

        public Spell CurrentSpell { get { return currentSpell; } }


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

            casting = false;
            animator.ResetTrigger("cast");
        }

        private void TriggerAttackCancel()
        {
            //Triggers Hit and Shoot methods
            animator.ResetTrigger("attack");
            animator.SetTrigger("attackCancel");


            animator.ResetTrigger("cast");
        }


        internal void TriggerSpellCast()
        {
            //Triggers Hit and Shoot methods
            casting = true;
            animator.SetTrigger("cast");
            animator.ResetTrigger("attack");
            animator.ResetTrigger("attackCancel");
        }

        void HitListener(GameObject target)
        {
			var targetsHealth = target.GetComponent<Health>();

			if (targetsHealth == null) return;

			ShowImpactEffect(target.transform);
			PlayImpactSound();
			target.GetComponent<Mover>().Knockback(transform.forward, currentWeapon.GetKnockback());
			targetsHealth.DealDamage(currentWeapon.GetDamage(), gameObject, true);
		}

        //Animation trigger
        void Hit()
        {
            Camera.main.GetComponent<CameraShake>().TriggerShake(currentWeapon.GetFeedbackShakePower());
            if (target == null) return;
            if (!CanAttackMeele(target)) return;

			var targetsHealth = target.GetComponent<Health>();            
			if (targetsHealth != null)
			{
                ShowImpactEffect(target);
                PlayImpactSound();
                target.GetComponent<Mover>().Knockback(transform.forward, currentWeapon.GetKnockback());
                targetsHealth.DealDamage(currentWeapon.GetDamage(), gameObject, true);
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
            if(casting)
			{
                if (target == null)
                {
                    currentSpell.LaunchProjectile(leftHand, transform.forward, gameObject);
                }
                else
                {
                    var targetHealth = target.GetComponent<Health>();
                    currentSpell.LaunchProjectile(leftHand, targetHealth, gameObject);
                }
                casting = false;
            }
            else
			{
                if (target == null)
                {
                    currentWeapon.LaunchProjectile(rightHand, leftHand, transform.forward, gameObject);
                }
                else
                {
                    var targetHealth = target.GetComponent<Health>();
                    currentWeapon.LaunchProjectile(rightHand, leftHand, targetHealth, gameObject);
                }
            }
		}

		internal bool CanAttack(float angle, float distance)
		{
			return angle <= InteractionAngle && distance < GetMaxRange();
		}

		internal bool CanAttackMeele(Transform targetEnemy)
        {
            var directionVector = targetEnemy.position - transform.position;
            var distance = directionVector.magnitude;
            var angle = Vector3.Angle(directionVector, transform.forward);
            return angle <= InteractionAngle && distance < currentWeapon.GetRange();
        }

        private float GetMaxRange()
		{
            var weaponRange = currentWeapon != null ? currentWeapon.GetRange() : 0f;
            var spellRange = currentSpell != null ? currentSpell.Range : 0f;
            return Mathf.Max(weaponRange, spellRange);
        }

        private void PlayImpactSound()
		{
            var impactSound = currentWeapon.GetImpactSound();
            if (impactSound == null) return;
            AudioSource.PlayClipAtPoint(impactSound, health.transform.position);
        }

        private void ShowImpactEffect(Transform targetTransform)
		{
            if (targetTransform == null) return;
            var effect = currentWeapon.GetImpactEffect();
            if (effect == null) return;
            var collider = targetTransform.GetComponent<CapsuleCollider>();

            var offset = collider != null ? Vector3.up * collider.height / 2f : Vector3.zero;

            Instantiate(effect, targetTransform.position + offset, targetTransform.rotation);
        }
    }
}

