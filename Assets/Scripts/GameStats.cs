using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Core;
using System;
using RPG.Combat;

public class GameStats : MonoBehaviour
{
	public static GameStats instance;

	private Health health;

	public float totalTime;
	public int hitsTaken;
	public bool swordOnly = true;
	public bool magicOnly = true;

	public int totalEnemies;
	public int killedEnemies;

	public string TotalMinutes
	{
		get
		{
			var seconds = (int)totalTime;
			var minutes = seconds % 60;
			return $"{minutes}:{seconds}";
		}
	}

	public bool AllContent => totalEnemies - killedEnemies == 0;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		var player = GameObject.FindGameObjectWithTag("Player");
		health = player.GetComponent<Health>();
		health.OnTakeDamage += IncreaseHits;
	}

	private void Update()
	{
		totalTime += Time.deltaTime;
	}

	private void OnDisable()
	{
		health.OnTakeDamage -= IncreaseHits;
	}

	private void IncreaseHits(int amount)
	{
		hitsTaken++;
	}

	//private void DisableSwordOnly()
	//{
	//	swordOnly = false;
	//}

	//private void DisableMagicOnly()
	//{
	//	magicOnly = false;
	//}
}
