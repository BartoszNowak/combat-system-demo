using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed;

        private Animator animator = null;
        private NavMeshAgent navMeshAgent = null;
        private ActionManager actionManager = null;

        void Start()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            actionManager = GetComponent<ActionManager>();

            navMeshAgent.updateRotation = false;
        }

        public void Move(float horizontal, float vertical)
		{
            var movement = new Vector3(horizontal, 0, vertical) * -1;
            var destination = transform.position + movement;

            if (movement.magnitude > 0 && !actionManager.HasActiveAction())
            {
                navMeshAgent.SetDestination(destination);
                InstantlyTurn(destination);
            }
            var localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);

            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        private void InstantlyTurn(Vector3 destination)
        {
            var direction = destination - transform.position;
            if (Mathf.Approximately(direction.magnitude, 0f)) return;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        #region Unused animation triggers
        void FootL()
        {

        }

        void FootR()
        {

        }
		#endregion
	}
}
