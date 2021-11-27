using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public class EnemyHealthBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform healthBarValue;
		[SerializeField]
		private Health health;

		private void Start()
		{
			GetComponentInChildren<Canvas>().enabled = !Mathf.Approximately(health.GetFraction(), 1f);
		}

		private void OnEnable()
		{
			if (health == null) return;

			health.OnTakeDamage += UpdateHealthBar;
			health.OnDeath += DestroyHealthBar;
		}

		private void OnDisable()
		{
			if (health == null) return;

			health.OnTakeDamage -= UpdateHealthBar;
			health.OnDeath -= DestroyHealthBar;
		}

		private void UpdateHealthBar(int damage)
		{
			float healthFraction = health.GetFraction();
			GetComponentInChildren<Canvas>().enabled = healthFraction < 1f;

			healthBarValue.localScale = new Vector3(healthFraction, 1, 1);
		}

		private void DestroyHealthBar()
		{
			Destroy(gameObject);
		}
	}
}
