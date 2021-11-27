using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private GameObject targetIndicatorPrefab = null;

    private GameObject targetIndicator;

    private void Start()
	{
        targetIndicator = Instantiate(targetIndicatorPrefab);

	}

	private void Update()
	{
		if(targetIndicator != null && targetIndicator.activeSelf)
		{
			var collider = GetComponent<CapsuleCollider>();
			if (collider == null) return;

			MoveToTargetPosition();
		}
	}

	private void MoveToTargetPosition()
	{
		Vector3 offset = new Vector3(2.1f, 0.015f, 0f);
		targetIndicator.transform.position = transform.position + offset;
	}

	public void SetAsTarget()
    {
		MoveToTargetPosition();
        targetIndicator.SetActive(true);
    }

    public void RemoveTarget()
    {
        targetIndicator.SetActive(false);
    }
}
