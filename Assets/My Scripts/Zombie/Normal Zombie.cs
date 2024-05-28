using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Brogue.Player;
using Random = UnityEngine.Random;

namespace Brogue.Zombie
{
    

[RequireComponent(typeof(NavMeshAgent))]
public class NormalZombie : BaseEnemyClass, BattleProperties, IZombieHitable, IZombieAttackPower
{
    // battle systems
    [SerializeField] protected float huntingDistance;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int attackPower;
    [SerializeField] protected int defense;
    // [SerializeField] private float attckAgentStopTime;
    public int currentHp;
    public int maxHp;
    // meshes
    protected SkinnedMeshRenderer skinnedMeshRender;
    
    protected bool isHunting;
    protected float minAttackingInterval;
    protected float attackingIntervalCounter;
    protected Animator animator;
    
    // zombie had dead once, so it's second death
    protected bool isDeadAgain;
    
    protected bool isWithinAttackRange;
    
    // animator params to string
    protected int animatorWalkingString;
    protected int animatorDyingString;
    protected int animatorAttackingString;

    private new void Awake()
    {
        base.Awake();
        

        isWithinAttackRange = false;
        isHunting = false;
        isDeadAgain = false;
        minAttackingInterval = 1 / attackSpeed;
        attackingIntervalCounter = 0f;
        


    }

    protected new virtual void Start()
    {
        base.Start();
        skinnedMeshRender = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
        animatorWalkingString = Animator.StringToHash("WalkingSpeed");
        animatorDyingString = Animator.StringToHash("dying");
        animatorAttackingString = Animator.StringToHash("attacking");
        skinnedMeshRender.sharedMesh = GameManager.Instance.aBloader.GetRandomZombieMesh("Zombie");
    }

    protected virtual void Update()
    {
        StopHuntIfPlayerOutOfHuntingRange();
        AttackPlayerIfWithingAttackingRange();
        CheckIfDead();
        
        // Debug.Log(agent.remainingDistance);
    }


    protected virtual void OnAnimatorMove()
    {
        transform.position += animator.deltaPosition;
        agent.nextPosition = transform.position;
        // Debug.Log(agent.nextPosition);
    }

    // 超出范围则停止hunt玩家 or within attack range
    protected virtual void StopHuntIfPlayerOutOfHuntingRange()
    {
        isHunting = Vector3.Distance(transform.position, target.transform.position) <= huntingDistance;
        if (isWithinAttackRange)
        {
            animator.SetFloat(animatorWalkingString, 0f);
            return;
        }
        if (isHunting)
        {
            NavObstacleChangeToAgent();
            agent.isStopped = false;
            animator.SetFloat(animatorWalkingString, Mathf.Clamp(agent.velocity.magnitude,0f, 1.5f));
            // animator.SetBool(animatorWalkingString, true);
        }
        else
        {
            agent.isStopped = true;
            NavAgentChangeToObstacle();
            animator.SetFloat(animatorWalkingString, Mathf.Clamp(agent.velocity.magnitude,0f, 1.5f));
            // animator.SetBool(animatorWalkingString, false);
        }
    }

    protected virtual void AttackPlayerIfWithingAttackingRange()
    {
        if (attackingIntervalCounter > 0f)
        {
            attackingIntervalCounter -= Time.deltaTime;
        }
        
        if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
        {
            if (!agent.enabled)
            {
                NavObstacleChangeToAgent();
            }

            isWithinAttackRange = false;
        }
        else
        {
            isWithinAttackRange = true;
            if (attackingIntervalCounter <= 0f)
            {
                Attack();
                attackingIntervalCounter = minAttackingInterval;
            }
            
        }

    }

    protected virtual void Attack()
    {
        NavAgentChangeToObstacle();
        PlayerMovement player = target.GetComponent<PlayerMovement>();
        // transform.LookAt(target);
        // player.currentHp -= attackPower;
        animator.SetTrigger(animatorAttackingString);
        
        // Debug.Log(name + " Attacks!");
    }

    private void CheckIfDead()
    {
        if (!isDeadAgain)
        {
            if (currentHp <= 0)
            {
                Dead();
            }
        
        }
    }
    // 如果在isHunting == false的时候，关掉了这个coroutine，那么target position不会更新，isHunting不再会自动变为true


    // private IEnumerator CoroStopAgentForSomeSeconds(float seconds)
    // {
    //     agent.isStopped = true;
    //     yield return new WaitForSeconds(seconds);
    //     agent.isStopped = false;
    // }


    public void GetHit(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
        {
            currentHp = 0;
        }
    }

    public Transform GetPosition()
    {
        return transform;
    }

    public void Dead()
    {
        if (generator is not null)
        {
            generator.zombieCount--;
            // Debug.Log("normal zombie die");
        }

        

        
        animator.SetTrigger(animatorDyingString);
        if (agent.enabled)
        {
            agent.isStopped = true;
        }
        isDeadAgain = true;
        StopCoroutine(navCoro);
        agent.enabled = false;
        Collider _collider = GetComponent<Collider>();
        Destroy(_collider);
        StartCoroutine(CoroDeadRemoveLastComponents());

    }



    public int Defense()
    {
        return defense;
    }

    int IZombieAttackPower.AttackPower()
    {
        return attackPower;
    }

    protected IEnumerator CoroDeadRemoveLastComponents()
    {
        foreach (var _component in GetComponents<Component>())
        {
            if (!(_component is Animator || _component is Transform || _component is NavMeshAgent))
            {
                // 尝试将组件转换为Behaviour类型（大部分可被禁用的组件都是从Behaviour派生的）
                Behaviour behaviourComponent = _component as Behaviour;
                if (behaviourComponent != null)
                {
                    Destroy(_component);// 禁用组件
                }
            }
        }
        yield return new WaitForSeconds(2);
        Destroy(animator);
        Destroy(agent);
        
    }
}
}