using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBattle : MonoBehaviour
{
    [SerializeField]
    private GameObject battle;
    [SerializeField]
    private GameObject portal;
    [SerializeField]
    private UIAnimationManager animationManager;

    private int spawnersAmount;

    private event Action OnFinish;

    void Start()
    {
        StartCoroutine(BattleStartDelay());
    }

	private void OnEnable()
	{
        OnFinish += FinishBattle;
	}

    private void OnDisable()
    {
        OnFinish -= FinishBattle;
    }

    private IEnumerator BattleStartDelay()
	{
        GameObject.FindGameObjectWithTag("Player").transform.rotation = Quaternion.Euler(0, 180, 0);
        Camera.main.GetComponent<CameraShake>().TriggerShake(1, 1);
        yield return new WaitForSeconds(1);

        portal.SetActive(true);
        Camera.main.GetComponent<CameraShake>().TriggerShake(0.5f, 1);
        yield return new WaitForSeconds(1);

        Camera.main.GetComponent<CameraShake>().TriggerShake(0.2f, 1);
        yield return new WaitForSeconds(1);

        battle.SetActive(true);

        var spawners = GameObject.FindGameObjectsWithTag("WaveSpawner");
        spawnersAmount = spawners.Length;

        foreach (var s in spawners)
		{
            s.GetComponent<EnemySpawnerPortal>().OnClear += RemoveSpawner;
		}
    }

    private void RemoveSpawner()
	{
        spawnersAmount -= 1;
        if(spawnersAmount == 0)
		{
            OnFinish?.Invoke();
		}
	}

    private void FinishBattle()
	{
        StartCoroutine(FinalSequence());
	}

    private IEnumerator FinalSequence()
	{
        battle.GetComponentInChildren<MagicTrapSpawner>().Disable();
        yield return new WaitForSeconds(1);
        animationManager.FadeScreen();
        yield return new WaitForSeconds(3);

        animationManager.OpenEndMenu();
    }
}
