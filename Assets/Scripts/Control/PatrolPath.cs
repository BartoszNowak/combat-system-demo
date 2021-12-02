using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), 0.3f);
                var nextIndex = GetNextIndex(i);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(nextIndex));
            }
        }

        public int GetNextIndex(int index)
        {
            return (index + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}
