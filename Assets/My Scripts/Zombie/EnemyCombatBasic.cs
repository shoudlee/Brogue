using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Brogue.Zombie
{
    public class BaseEnemyClass : MonoBehaviour
    {
        public int mapRange = 50;
        
        
        // interval must not be 0
        [SerializeField] protected Transform target;
        protected float navTargetPositionUpdateInterval;
        protected NavMeshAgent agent;
        protected NavMeshObstacle obstacle;
        protected Coroutine navCoro;
        protected ZombieGenerator generator;
        protected bool isDeadAgain;
        


        protected void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            obstacle = GetComponent<NavMeshObstacle>();
            agent.updatePosition = false;
            isDeadAgain = false;
            // agent.updateRotation = false;
        }

        protected void Start()
        {
            
            agent.SetDestination(target.position);
            navCoro = StartCoroutine(CoroUpdateNavTargetPosition());
            if (Math.Abs(transform.position.x) > mapRange-5 || Math.Abs(transform.position.z) > mapRange-5)
            {
                Destroy(gameObject);
            }
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
            if (isDeadAgain)
            {
                return;
            }
            agent.enabled = false;
            obstacle.enabled = true;
            obstacle.carving = true;
        }
        
        protected void NavObstacleChangeToAgent()
        {
            if (isDeadAgain)
            {
                return;
            }
            obstacle.enabled = false;
            agent.enabled = true;
            obstacle.carving = false;
        }

        public void Init(Transform target, ZombieGenerator generator)
        {
            this.target = target;
            this.generator = generator;
        }

        private void OnDestroy()
        {
            ZombieAttackHands[] zombieAttackHands = GetComponentsInChildren<ZombieAttackHands>();
            foreach (ZombieAttackHands hand in zombieAttackHands)
            {
                hand.gameObject.SetActive(false);
            }            

        }
        
    }
}