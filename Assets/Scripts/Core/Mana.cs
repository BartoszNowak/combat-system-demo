using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Mana : MonoBehaviour
    {
        [SerializeField]
        private int maxMana;

        public float Fraction => currentMana / (float)maxMana;

        private int currentMana;

        private float manaRegenerationFraction = 0f;
        private float manaRegenerationAcceleration = 1f;

        void Start()
        {
            currentMana = maxMana;
        }

        void Update()
        {
            if (currentMana == maxMana) return;

            manaRegenerationAcceleration += Time.deltaTime;
            manaRegenerationFraction += Time.deltaTime * manaRegenerationAcceleration;
            if(manaRegenerationFraction > 1)
			{
                currentMana += 1;
                manaRegenerationFraction = 0f;
			}
        }

        public void IncreaseMana(int amount)
        {
            currentMana = Mathf.Min(currentMana + amount, maxMana);
        }

        public void DecreaseMana(int amount)
		{
            currentMana = Mathf.Max(currentMana - amount, 0);
            manaRegenerationAcceleration = 1f;
		}

        public bool CanCast(int mana)
		{
            return currentMana >= mana;
		}
    }
}
