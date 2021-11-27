using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI
{
	public class DamageTextSpawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject damageText;
		[SerializeField]
		private Health health;

		private void OnEnable()
		{
			health.OnTakeDamage += Spawn;
		}

		private void OnDisable()
		{
			health.OnTakeDamage -= Spawn;
		}

		public void Spawn(int damage)
		{
			damageText.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = $"{damage}";
			Instantiate(damageText, gameObject.transform);
		}
	}
}
