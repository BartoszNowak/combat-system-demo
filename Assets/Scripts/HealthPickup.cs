using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    private int healAmount = 30;
    [SerializeField]
    private float timeToDestroy = 10f;
    [SerializeField]
    private GameObject healEffect = null;
    [SerializeField]
    private AudioClip healSound = null;

	private void Start()
	{
        Destroy(gameObject, timeToDestroy);
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag != "Player") return;

        var health = other.GetComponent<Health>();
        if (health.GetFraction() == 1f) return;
        
        health.Heal(healAmount);
        Instantiate(healEffect, other.transform);
        other.GetComponent<AudioSource>().PlayOneShot(healSound);
        Destroy(gameObject);
	}
}
