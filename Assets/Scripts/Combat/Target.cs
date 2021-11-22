using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private GameObject targetIndicator = null;

    public void SetAsTarget()
    {
        targetIndicator.SetActive(true);
    }

    public void RemoveTarget()
    {
        targetIndicator.SetActive(false);
    }
}
