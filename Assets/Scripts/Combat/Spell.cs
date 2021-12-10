using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Spell", menuName = "Spells/New Spell", order = 0)]
    public class Spell : ScriptableObject
    {
        [SerializeField]
        private float range = 1f;
        [SerializeField]
        private int damage = 10;
        [SerializeField]
        private int manaCost = 10;
        [SerializeField]
        private float timeBetweenCasts = 2f;
        [SerializeField]
        private AnimatorOverrideController spellOverride = null;
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

        public void Attune(Animator animator)
        {
            //OverrideAnimations(animator);
        }

        private void OverrideAnimations(Animator animator)
        {
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (spellOverride != null)
            {
                animator.runtimeAnimatorController = spellOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

            }
        }

        public void LaunchProjectile(Transform leftHand, Health target, GameObject attacker)
        {
            var projectile = Instantiate(projectilePrefab, leftHand.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTarget(target, damage, attacker);
        }

        public void LaunchProjectile(Transform leftHand, Vector3 direction, GameObject attacker)
        {
            var projectile = Instantiate(projectilePrefab, leftHand.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetDirection(direction, damage, attacker);
        }

        public float Range { get { return range; } }
        public int Damage { get { return damage; } }
        public int ManaCost { get { return manaCost; } }
        public float TimeBetweenCasts { get { return timeBetweenCasts; } }
        public bool HasHyperArmor { get { return hasHyperArmor; } }
        public float Knockback { get { return knockback; } }
        public float FeedbackShakePower { get { return feedbackShakePower; } }
        public AudioClip AttackSound { get { return attackSound; } }
        public GameObject ImpactEffect { get { return impactEffect; } }
        public AudioClip ImpactSound { get { return impactSound; } }
    }
}
