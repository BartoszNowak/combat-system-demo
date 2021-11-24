using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class AIMover : Mover
    {
        //private Animator animator;
        //private NavMeshAgent agent;
        //private ActionManager actionManager;
        //private Health health;


        //void Start()
        //{
        //    animator = GetComponent<Animator>();
        //    actionManager = GetComponent<ActionManager>();
        //    agent = GetComponent<NavMeshAgent>();
        //    health = GetComponent<Health>();

        //}

        void Update()
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            var localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }
    }
}
