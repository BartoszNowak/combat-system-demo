using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    private int healAmount = 30;
    [SerializeField]
    private GameObject healEffect = null;
    [SerializeField]
    private AudioClip healSound = null;

	private void Start()
	{
        Destroy(gameObject, 10f);
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag != "Player") return;

        var health = other.GetComponent<Health>();
        if (health.GetFraction() == 1f) return;
        
        health.Heal(healAmount);
        Instantiate(healEffect, other.transform);
        AudioSource.PlayClipAtPoint(healSound, Camera.main.transform.position);
        Destroy(gameObject);
	}
}
