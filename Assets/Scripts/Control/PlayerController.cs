using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
	[RequireComponent(typeof(Mover))]
	public class PlayerController : MonoBehaviour
	{
		private Mover movement;
		private Attacker combatAgent;
		private Health health;

		void Start()
		{
			movement = GetComponent<Mover>();
			combatAgent = GetComponent<Attacker>();
			health = GetComponent<Health>();
		}

		void Update()
		{
			if (health.IsDead()) return;

			HandleMovement();
			HandleCombat();
		}

		private void HandleCombat()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				combatAgent.Attack();
			}
		}

		private void HandleMovement()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");
			movement.Move(horizontal, vertical);
		}
	}
}