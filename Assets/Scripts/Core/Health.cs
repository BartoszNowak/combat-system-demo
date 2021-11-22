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

        public delegate void TakeDamage(int damage);
        public event TakeDamage OnTakeDamage; 
        public event Action OnDeath; 

        private void Awake()
        {
            actionManager = GetComponent<ActionManager>();
            currentHealth = maxHealth;
        }

		public void DealDamage(int damage, GameObject attacker)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            Debug.Log(currentHealth);
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
    }
}
