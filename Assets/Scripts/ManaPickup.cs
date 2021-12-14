using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickup : MonoBehaviour
{
    [SerializeField]
    private int gainAmount = 50;
    [SerializeField]
    private float timeToDestroy = 10f;
    [SerializeField]
    private GameObject manaEffect = null;
    [SerializeField]
    private AudioClip manaSound = null;

	private void Start()
	{
        Destroy(gameObject, timeToDestroy);
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag != "Player") return;

        var mana = other.GetComponent<Mana>();
        if (mana.Fraction == 1f) return;
        
        mana.IncreaseMana(gainAmount);
        Instantiate(manaEffect, other.transform);
        other.GetComponent<AudioSource>().PlayOneShot(manaSound);
        Destroy(gameObject);
	}
}
