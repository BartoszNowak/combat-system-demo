using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelReload : MonoBehaviour
{
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private float transitionTime = 1f;

    private Health playerHealth;

    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

	private void OnEnable()
	{
		playerHealth.OnDeath += TriggerLevelReload;
	}

	private void OnDisable()
	{
		playerHealth.OnDeath -= TriggerLevelReload;
	}

	private void TriggerLevelReload()
	{
		StartCoroutine(ReloadLevel());
	}

	private IEnumerator ReloadLevel()
	{
		animator.SetTrigger("start");

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
