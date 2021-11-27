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

        internal Animator animator = null;
        internal NavMeshAgent navMeshAgent = null;
        internal ActionManager actionManager = null;
        private float maxSpeed;

        internal Health health;

        void Start()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            actionManager = GetComponent<ActionManager>();

            maxSpeed = navMeshAgent.speed;

            health = GetComponent<Health>();
        }

		private void Update()
		{
            navMeshAgent.enabled = !health.IsDead();
            UpdateLocomotionAnimation();
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
        }

        public void MoveTo(Vector3 destination, float maxSpeedFraction)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.speed = maxSpeed * maxSpeedFraction;
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = true;
        }

        public void MoveTo(Vector3 destination)
        {
            MoveTo(destination, 1f);
        }

        public void Knockback(Vector3 direction, float strength)
		{
            var knockbackOffset = direction * strength;
            navMeshAgent.SetDestination(transform.position + knockbackOffset);
            navMeshAgent.Move(knockbackOffset);
        }

        public void Stop()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.updateRotation = false;
        }

        private void UpdateLocomotionAnimation()
        {
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
