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

    private const float damageFlashTime = 0.2f;
    private float damageFlashEllapsedTime = Mathf.Infinity;
	private Material originalMaterial;

	private void Awake()
	{
        health = GetComponent<Health>();
        originalMaterial = modelRenderer.material;
    }

	private void OnEnable()
	{
        health.OnTakeDamage += Show;
	}

    private void OnDisable()
    {
        health.OnTakeDamage -= Show;
    }

	//private void Update()
	//{
 //       if (originalMaterial == null) return;

 //       damageFlashEllapsedTime += Time.deltaTime;
 //       if (damageFlashEllapsedTime > damageFlashTime)
 //       {
 //           modelRenderer.material = originalMaterial;
 //       }
 //   }

	public void Show(int damage)
    {
        //      if(originalMaterial == null)
        //{
        //          originalMaterial = modelRenderer.material;
        //      }
        //      damageFlashEllapsedTime = 0f;
        //      modelRenderer.material = damageFlashMaterial;

        StartCoroutine(ShowDamageFlash());
    }

    private IEnumerator ShowDamageFlash()
	{
        modelRenderer.material = damageFlashMaterial;

        yield return new WaitForSeconds(0.2f);

        modelRenderer.material = originalMaterial;
    }

    private IEnumerator ShowDamageFlashOld()
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
