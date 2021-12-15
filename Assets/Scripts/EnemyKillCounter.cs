using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillCounter : MonoBehaviour
{
    private GameStats stats;
    private Health health;

    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<GameStats>();
        health = GetComponent<Health>();
        health.OnDeath += CountKill;
        stats.totalEnemies++;
    }

    private void OnDestroy()
    {
        health.OnDeath -= CountKill;
    }

    private void CountKill()
	{
        stats.killedEnemies++;
	}
}
