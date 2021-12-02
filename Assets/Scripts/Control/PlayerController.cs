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
		private Attacker attacker;
		private Health health;

		[SerializeField]
		private Weapon[] weapons;

		void Start()
		{
			movement = GetComponent<Mover>();
			attacker = GetComponent<Attacker>();
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
				attacker.Attack();
			}
			for (int i = 0; i < weapons.Length; i++)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1 + i))
				{
					attacker.EquipWeapon(weapons[i]);
				}
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