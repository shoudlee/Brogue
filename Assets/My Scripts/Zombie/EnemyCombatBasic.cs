using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Brogue.Zombie
{
    public class BaseEnemyClass : MonoBehaviour
    {
        // interval must not be 0
        [SerializeField] protected Transform target;
        protected float navTargetPositionUpdateInterval;
        protected NavMeshAgent agent;
        protected NavMeshObstacle obstacle;
        protected Coroutine navCoro;
        protected ZombieGenerator generator;

        protected void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            obstacle = GetComponent<NavMeshObstacle>();
            agent.updatePosition = false;
            // agent.updateRotation = false;
        }

        protected void Start()
        {
            agent.SetDestination(target.position);
            navCoro = StartCoroutine(CoroUpdateNavTargetPosition());
        }
        
        private IEnumerator CoroUpdateNavTargetPosition()
        {
        
            while (true)
            {
                if (agent.enabled && !agent.isStopped)
                {
                    agent.SetDestination(target.position);
                }
                yield return new WaitForSeconds(navTargetPositionUpdateInterval);

            }
        }
        protected void NavAgentChangeToObstacle()
        {
            agent.enabled = false;
            obstacle.enabled = true;
            obstacle.carving = true;
        }
        
        protected void NavObstacleChangeToAgent()
        {
            obstacle.enabled = false;
            agent.enabled = true;
            obstacle.carving = false;
        }
        public void Init(Transform target)
        {
            this.target = target;
        }
        public void Init(Transform target, ZombieGenerator generator)
        {
            this.target = target;
            this.generator = generator;
        }
    }
}