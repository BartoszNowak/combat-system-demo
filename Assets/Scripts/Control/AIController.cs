using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(AIAttacker))]
    [RequireComponent(typeof(Health))]
    public class AIController : MonoBehaviour
    {
        private const float InteractionAngle = 70f;

        [SerializeField]
        private float chaseDistance = 5f;
        [SerializeField]
        private float suspiciousTime = 10f;
        [SerializeField]
        private float waypointDistanceTolerance = 0.1f;
        [SerializeField]
        private float dwellingTime = 2f;
        [SerializeField]
        private float patrolSpeedMultiplier = 0.2f;
        [SerializeField]
        private float aggroTime = 10f;
        [SerializeField]
        private float callNerbyEnemiesDistance = 10f;
        //[SerializeField]
        //private PatrolPath patrolPath;

        private Mover movement;
        private AIAttacker combatAgent;
        private Health health;

        private GameObject player;
        private Vector3 guardLocation;
        private int currentWaypointIndex = 0;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeAtWaypoint = Mathf.Infinity;
        private float aggrevatedTimer = Mathf.Infinity;

		private void OnEnable()
		{
            health.OnTakeDamage += Aggrevate;
		}

        private void OnDisable()
        {
            health.OnTakeDamage -= Aggrevate;
        }

        private void Awake()
        {
            guardLocation = transform.position;
            player = GameObject.FindGameObjectWithTag("Player");
            movement = GetComponent<Mover>();
            combatAgent = GetComponent<AIAttacker>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            if (IsPlayerDead() || IsThisEnemyDead()) return;

            if (IsPlayerInAttackRange() || aggrevatedTimer < aggroTime)
            {
                AttackingBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspiciousTime)
            {
                SuspiciousBehaviour();
            }
            else
            {
                GuardingBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            
        }

		private void Aggrevate(int damage)
		{
            aggrevatedTimer = 0f;
		}

		private void AttackingBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
            aggrevatedTimer += Time.deltaTime;
            combatAgent.Attack(player);

            CallNerbyEnemies();
        }

		private void CallNerbyEnemies()
		{
            var hits = Physics.SphereCastAll(transform.position, callNerbyEnemiesDistance, Vector3.up, 0);
            foreach(var hit in hits)
			{
                var enemy = hit.transform.GetComponent<AIController>();
                if(enemy != null)
				{
                    enemy.Aggrevate(0);
                }
            }
		}

		private void SuspiciousBehaviour()
        {
            GetComponent<ActionManager>().CancelCurrentAction();
        }

        private void GuardingBehaviour()
        {
            Vector3 nextPosition = guardLocation;

            //if(patrolPath != null)
            //{
            //    nextPosition = patrolPath.GetWaypoint(currentWaypointIndex);

            //    if (Vector3.Distance(transform.position, nextPosition) < waypointDistanceTolerance)
            //    {
            //        if(timeAtWaypoint >= dwellingTime)
            //        {
            //            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
            //            timeAtWaypoint = 0;
            //        }
            //        else
            //        {
            //            timeAtWaypoint += Time.deltaTime;
            //        }
            //    }
            //}

            movement.MoveTo(nextPosition, patrolSpeedMultiplier);
        }

        private bool IsPlayerInAttackRange()
        {
            var directionVector = player.transform.position - transform.position;
            var distance = directionVector.magnitude;
            var angle = Vector3.Angle(directionVector, transform.forward);

            return distance <= chaseDistance;// && angle <= InteractionAngle;
        }

        private bool IsPlayerDead()
        {
            return player.GetComponent<Health>().IsDead();
        }

        private bool IsThisEnemyDead()
        {
            return health.IsDead();
        }
    }

}