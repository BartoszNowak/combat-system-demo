using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
	[SerializeField]
	private Drop[] dropList;

    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
    }

	private void OnEnable()
	{
		health.OnDeath += DropItem;
	}

	private void OnDisable()
	{
		health.OnDeath -= DropItem;
	}

	private void DropItem()
	{
		if (dropList.Length == 0) return;

		foreach (var d in dropList)
		{
			var number = Random.Range(0, 100);
			if (number < 100 - d.probability * 100) continue;

			var yRot = Random.Range(0, 360);
			Instantiate(d.item, transform.position + Vector3.up, Quaternion.Euler(0, yRot, 0));
		}
	}
}

[System.Serializable]
public class Drop
{
	public GameObject item;
	[Range(0f, 1f)]
	public float probability;
}
