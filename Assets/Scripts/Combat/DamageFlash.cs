using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField]
    private Health health = null;
    [SerializeField]
    private Material damageFlashMaterial = null;

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
        var renderer = GetComponent<SkinnedMeshRenderer>();
        var materialsOriginal = new Material[renderer.materials.Length];
        renderer.materials.CopyTo(materialsOriginal, 0);

        var materialsReplace = new Material[renderer.materials.Length + 1];
        renderer.materials.CopyTo(materialsReplace, 0);
        Debug.Log($"Original = {materialsOriginal.Length}, replacement = {materialsReplace.Length}");

        materialsReplace[renderer.materials.Length - 1] = damageFlashMaterial;
        renderer.materials = materialsReplace;
        yield return new WaitForSeconds(0.1f);
        renderer.materials = materialsOriginal;
    }
}
