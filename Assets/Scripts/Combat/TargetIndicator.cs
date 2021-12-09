using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
	private Transform target;
	private void Update()
	{
		if(gameObject != null && gameObject.activeSelf)
		{
			var collider = GetComponent<CapsuleCollider>();
			if (collider == null) return;

			MoveToTargetPosition();
		}
	}

	private void MoveToTargetPosition()
	{
		Vector3 offset = new Vector3(2.1f, 0.015f, 0f);
		gameObject.transform.position = target.position + offset;
	}

	public void SetAsTarget(Transform targetTransform)
    {
		target = targetTransform;
		MoveToTargetPosition();
        gameObject.SetActive(true);
    }

    public void RemoveTarget()
    {
		target = null;
		gameObject.SetActive(false);
    }
}
