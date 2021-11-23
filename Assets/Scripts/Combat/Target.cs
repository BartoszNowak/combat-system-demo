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
        targetIndicator.SetActive(false);

	}

	private void Update()
	{
		if(targetIndicator != null && targetIndicator.activeSelf)
		{
            var collider = GetComponent<CapsuleCollider>();
            if (collider == null) return;

			Vector3 offset = new Vector3(1, 0.5f, 0.6f);
			var displayPosition = transform.position + offset + (Vector3.up * collider.height / 2f);
            targetIndicator.transform.position = displayPosition;
        }
	}

	public void SetAsTarget()
    {
        targetIndicator.SetActive(true);
    }

    public void RemoveTarget()
    {
        targetIndicator.SetActive(false);
    }
}
