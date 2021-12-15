using RPG.Combat;
using RPG.Control;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPortal : MonoBehaviour
{
    [SerializeField]
    private WaveConfig waveConfig;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private GameObject spawnEffect;
    [SerializeField]
    private AudioClip spawnSound;

    private bool waveActive = true;
    private int enemiesInWave;
    private List<GameObject> enemyBodies = new List<GameObject>();

    private int currentWave = 0;

    public event Action OnClear;

    void Start()
    {
        StartCoroutine(Loop());
    }

	private IEnumerator Loop()
    {
        while (waveActive)
        {
            var wave = waveConfig.GetWaveData(currentWave);
            currentWave++;
            if (wave == null)
            {
                waveActive = false;
                OnClear?.Invoke();
                break;
            }

            enemiesInWave = wave.amount;

            Instantiate(spawnEffect, spawnPoint.position, Quaternion.identity);
            var aud = GameObject.Find("Battle").GetComponent<AudioSource>();
            aud.Play();

            yield return new WaitForSeconds(2);

            for (int i = 0; i < enemiesInWave; i++)
            {
                var enemy = Instantiate(wave.enemyPrefab, spawnPoint.position, Quaternion.identity);
                enemy.GetComponent<AIController>().chaseDistance = 40;
                enemy.GetComponent<Health>().OnDeath += Remove;
                enemyBodies.Add(enemy);
            }

            yield return new WaitUntil(() => enemiesInWave == 0);

            StartCoroutine(RemoveBodies());

            yield return new WaitForSeconds(2);
        }


    }

    private void Remove()
	{
        enemiesInWave -= 1;
	}

    private IEnumerator RemoveBodies()
	{
        yield return new WaitForSeconds(3);
        foreach (var body in enemyBodies)
		{
            body.GetComponent<Health>().OnDeath -= Remove;
            Destroy(body);
        }
        enemyBodies.Clear();
	}
}
