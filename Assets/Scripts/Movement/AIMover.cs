using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ActionManager))]
    [RequireComponent(typeof(Health))]
    public class AIMover : MonoBehaviour
    {
        private Animator animator;
        private NavMeshAgent agent;
        private ActionManager actionManager;
        private Health health;

        private float maxSpeed;

        void Start()
        {
            animator = GetComponent<Animator>();
            actionManager = GetComponent<ActionManager>();
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();

            maxSpeed = agent.speed;
        }

        void Update()
        {
            agent.enabled = !health.IsDead();

            UpdateAnimation();
        }

        public void StartMoveAction(Vector3 destination)
        {
            //actionManager.StartAction(this);
            MoveTo(destination, 1f);
        }

        public void StartMoveAction(Vector3 destination, float maxSpeedFraction)
        {
            //actionManager.StartAction(this);
            MoveTo(destination, maxSpeedFraction);
        }

        public void MoveTo(Vector3 destination, float maxSpeedFraction)
        {
            agent.SetDestination(destination);
            agent.speed = maxSpeed * maxSpeedFraction;
            agent.isStopped = false;
            agent.updateRotation = true;
        }

        public void MoveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
            agent.speed = maxSpeed;
            agent.isStopped = false;
            agent.updateRotation = true;
        }

        public void Stop()
        {
            agent.isStopped = true;
            agent.updateRotation = false;
        }

        private void UpdateAnimation()
        {
            var localVelocity = transform.InverseTransformDirection(agent.velocity);
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void Cancel()
        {
            Stop();
        }

        void FootL()
        {

        }

        void FootR()
        {

        }
    }
}
