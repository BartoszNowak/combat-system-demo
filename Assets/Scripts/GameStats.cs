using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Core;
using System;
using RPG.Combat;

public class GameStats : MonoBehaviour
{
	private Health health;
	private PlayerAttacker playerAttacker;

	private float totalTime;
	private int hitsTaken;
	private bool allContent;
	private bool swordOnly;
	private bool magicOnly;

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
		var player = GameObject.FindGameObjectWithTag("Player");
		health = player.GetComponent<Health>();
		health.OnTakeDamage += IncreaseHits;
		playerAttacker = player.GetComponent<PlayerAttacker>();
		playerAttacker.OnFailSwordOnly += DisableSwordOnly;
		playerAttacker.OnFailMagicOnly += DisableMagicOnly;
	}

	private void OnDisable()
	{
		health.OnTakeDamage -= IncreaseHits;
	}

	private void IncreaseHits(int amount)
	{
		hitsTaken++;
	}

	private void DisableSwordOnly()
	{
		swordOnly = false;
	}

	private void DisableMagicOnly()
	{
		magicOnly = false;
	}
}
