using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform healthBarValue;
		[SerializeField]
		private RectTransform lastHealthBarValue;
		private Health health;

		private void Awake()
		{
			health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
		}

		private void Update()
		{
			if (healthBarValue.localScale.x >= lastHealthBarValue.localScale.x) return;

			var newScale = lastHealthBarValue.localScale.x - Time.deltaTime * 0.1f;
			lastHealthBarValue.localScale = new Vector3(newScale, 1, 1);
		}

		private void OnEnable()
		{
			health.OnTakeDamage += UpdateHealthBar;
			health.OnDeath += DestroyHealthBar;
		}

		private void OnDisable()
		{
			health.OnTakeDamage -= UpdateHealthBar;
			health.OnDeath -= DestroyHealthBar;
		}

		private void UpdateHealthBar(int damage)
		{
			float healthFraction = health.GetFraction();
			healthBarValue.localScale = new Vector3(healthFraction, 1, 1);
		}

		private void DestroyHealthBar()
		{
			Destroy(gameObject);
		}
	}
}
