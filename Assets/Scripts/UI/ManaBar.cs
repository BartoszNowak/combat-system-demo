using RPG.Combat;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public class ManaBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform value;
		[SerializeField]
		private CanvasGroup canvasGroup;
		[SerializeField]
		private AudioClip warningSound;

		private Mana mana;
		private PlayerAttacker attacker;

		private void Awake()
		{
			var player = GameObject.FindGameObjectWithTag("Player");
			mana = player.GetComponent<Mana>();
			attacker = player.GetComponent<PlayerAttacker>();
		}

		private void Update()
		{
			canvasGroup.alpha -= Time.deltaTime * 2;
			float fraction = mana.Fraction;
			value.localScale = new Vector3(fraction, 1, 1);
		}

		private void OnEnable()
		{
			attacker.OnManaWarning += ShowWarning;
		}

		private void OnDisable()
		{
			attacker.OnManaWarning -= ShowWarning;
		}

		private void ShowWarning()
		{
			canvasGroup.alpha = 0.5f;
			AudioSource.PlayClipAtPoint(warningSound, Camera.main.transform.position);
		}
	}
}
