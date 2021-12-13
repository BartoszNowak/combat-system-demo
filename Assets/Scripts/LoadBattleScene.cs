using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBattleScene : MonoBehaviour
{
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private float transitionTime = 1f;

	private void OnTriggerEnter(Collider other)
	{
		StartCoroutine(LoadBattleLevel());
	}

	private IEnumerator LoadBattleLevel()
	{
		animator.SetTrigger("start");

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
