using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        private float range = 1f;
        [SerializeField]
        private int damage = 10;
        [SerializeField]
        private float timeBetweenAttacks = 2f;
        [SerializeField]
        private GameObject weaponPrefab = null;
        [SerializeField]
        private AnimatorOverrideController weaponOverride = null;
        [SerializeField]
        private bool isRightHanded = true;
        [SerializeField]
        private bool canCancelAnimation = true;
        [SerializeField]
        private bool hasHyperArmor = false;
        [SerializeField]
        private float knockback = 0f;
        [SerializeField]
        private float feedbackShakePower = 0f;
        [SerializeField]
        private GameObject projectilePrefab = null;
        [SerializeField]
        private AudioClip attackSound = null;
        [SerializeField]
        private GameObject impactEffect = null;
        [SerializeField]
        private AudioClip impactSound = null;

        public float GetRange()
		{
            return range;
		}
        public int GetDamage()
		{
            return damage;
		}
        public float GetTimeBetweenAttacks()
		{
            return timeBetweenAttacks;
		}
        public bool CanCancelAnimation()
		{
            return canCancelAnimation;
		}
        public bool HasHyperArmor()
        {
            return hasHyperArmor;
        }
        public float GetKnockback()
		{
            return knockback;
		}
        public float GetFeedbackShakePower()
        {
            return feedbackShakePower;
        }
        public AudioClip GetAttackSound()
		{
            return attackSound;

        }
        public GameObject GetImpactEffect()
        {
            return impactEffect;

        }
        public AudioClip GetImpactSound()
        {
            return impactSound;

        }

        const string weaponName = "Weapon";

        private GameObject weaponInstance;

        public HitBoxTrigger HitBox => weaponInstance.GetComponent<HitBoxTrigger>();

        public void EnableHitBox(bool enable)
		{
            weaponInstance.GetComponent<CapsuleCollider>().enabled = enable;
		}

        public void Equip(Transform rightHand, Transform leftHand, Animator animator)
		{
			DestroyOldWeapon(rightHand, leftHand);
			SpawnWeapon(rightHand, leftHand);
			OverrideAnimations(animator);
		}

		public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject attacker)
		{
			var projectile = Instantiate(projectilePrefab, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
			projectile.GetComponent<Projectile>().SetTarget(target, damage, attacker);
		}

		public void LaunchProjectile(Transform rightHand, Transform leftHand, Vector3 direction, GameObject attacker)
		{
			var projectile = Instantiate(projectilePrefab, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
			projectile.GetComponent<Projectile>().SetDirection(direction, damage, attacker);
		}

		private void SpawnWeapon(Transform rightHand, Transform leftHand)
		{
			if (weaponPrefab != null)
			{
				var handTransform = GetHandTransform(rightHand, leftHand);
				weaponInstance = Instantiate(weaponPrefab, handTransform);
				weaponInstance.name = weaponName;
			}
		}

		private void OverrideAnimations(Animator animator)
		{
			var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

			if (weaponOverride != null)
			{
				animator.runtimeAnimatorController = weaponOverride;
			}
			else if (overrideController != null)
			{
				animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

			}
		}

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }
    }
}
