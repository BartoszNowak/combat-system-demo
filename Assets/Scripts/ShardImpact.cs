using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardImpact : MonoBehaviour
{
	[SerializeField]
	private int damage = 20;
	[SerializeField]
	private float shakeScreenPower = 2f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player") return;

		Camera.main.GetComponent<CameraShake>().TriggerShake(shakeScreenPower, 0.5f);
		var targetsHealth = other.GetComponent<Health>();
		targetsHealth.DealDamage(damage, gameObject, false);

		Destroy(gameObject);
	}
}
