using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer modelRenderer = null;
    [SerializeField]
    private Material damageFlashMaterial = null;

    private Health health;

    private void Awake()
	{
        health = GetComponent<Health>();
    }

	private void OnEnable()
	{
        health.OnTakeDamage += Show;
	}

    private void OnDisable()
    {
        health.OnTakeDamage -= Show;
    }

    public void Show(int damage)
    {
        StartCoroutine(ShowDamageFlash());
    }

    private IEnumerator ShowDamageFlash()
	{
        var materialsOriginal = new Material[modelRenderer.materials.Length];
        modelRenderer.materials.CopyTo(materialsOriginal, 0);

        var materialsReplace = new Material[modelRenderer.materials.Length + 1];
        modelRenderer.materials.CopyTo(materialsReplace, 0);

        materialsReplace[modelRenderer.materials.Length - 1] = damageFlashMaterial;
        modelRenderer.materials = materialsReplace;
        yield return new WaitForSeconds(0.2f);
        modelRenderer.materials = materialsOriginal;
    }
}
