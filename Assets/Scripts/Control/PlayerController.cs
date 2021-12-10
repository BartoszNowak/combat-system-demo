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
			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
			{
				attacker.Attack();
			}
			if (Input.GetMouseButtonDown(1))
			{
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				var hasHit = Physics.Raycast(ray, out hit);
				var target = hasHit ? hit.point : Vector3.zero;
				if(hasHit)
				{
					Debug.DrawRay(ray.origin, ray.direction * 100);
				}
				var direction = target - transform.position;
				attacker.CastSpell(direction);
			}
			if(Input.GetKeyDown(KeyCode.LeftShift))
			{
				attacker.CastSpell(Vector3.zero);
			}
			//for (int i = 0; i < weapons.Length; i++)
			//{
			//	if (Input.GetKeyDown(KeyCode.Alpha1 + i))
			//	{
			//		attacker.EquipWeapon(weapons[i]);
			//	}
			//}
		}

		private void HandleMovement()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");
			movement.Move(horizontal, vertical);
		}
	}
}