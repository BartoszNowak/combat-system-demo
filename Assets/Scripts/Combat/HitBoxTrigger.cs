using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxTrigger : MonoBehaviour
{
	public delegate void HitDetected(GameObject target);
	public event HitDetected OnHit;

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log($"{other.name} entered trigger!");
		if (other.tag != "Enemy") return;

		OnHit?.Invoke(other.gameObject);
	}
}
