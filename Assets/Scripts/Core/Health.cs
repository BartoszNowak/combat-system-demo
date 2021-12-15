using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    [RequireComponent(typeof(ActionManager))]
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth = 100;

        private ActionManager actionManager;

        private int currentHealth;
        private bool isDead;

        public delegate void HealthChanged(int amount);
        public event HealthChanged OnTakeDamage;
        public event HealthChanged OnHeal;
        public event Action OnDeath;

        private void Awake()
        {
            actionManager = GetComponent<ActionManager>();
            currentHealth = maxHealth;
        }

		public void DealDamage(int damage, GameObject attacker, bool melee)
        {
            SetStatsManagerValues(attacker, melee);
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            OnTakeDamage?.Invoke(damage);
            if (currentHealth == 0)
            {
                Die();
            }
            else
			{
                GetComponent<Animator>().SetTrigger("hit");
            }
        }

        public void Heal(int amount)
		{
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHeal?.Invoke(amount);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public float GetFraction()
		{
            return currentHealth / (float)maxHealth;
		}

		private void Die()
        {
            if (isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            var collider = GetComponent<CapsuleCollider>();
            if(collider != null)
			{
                collider.enabled = false;
			}

            actionManager.CancelCurrentAction();
            isDead = true;
            OnDeath?.Invoke();
        }

        public void GainIFrames(Animator animator)
        {
            StartCoroutine(TakingDamage(animator));
        }

        private IEnumerator TakingDamage(Animator animator)
        {
            yield return new WaitForSeconds(1);
            animator.ResetTrigger("hit");
            animator.SetBool("takingDamage", false);
        }

        private void SetStatsManagerValues(GameObject attacker, bool melee)
		{
            if (attacker.tag == "Player")
            {
                var stats = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<GameStats>();
                if (melee)
                {
                    stats.magicOnly = false;
                }
                else
                {
                    stats.swordOnly = false;
                }
            }
        }
    }
}
