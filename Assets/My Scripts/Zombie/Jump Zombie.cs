using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Core;
using Brogue.Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace  Brogue.Zombie{
public class JumpZombie : BaseEnemyClass, BattleProperties, IZombieHitable, IZombieAttackPower
{
     
    // battle systems
    [SerializeField] private float huntingDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int attackPower;
    [SerializeField] private int defense;
    [SerializeField] private float jumpAttackCoolDown;
    [SerializeField] private int jumpAttackRange;

    [SerializeField] private Transform jumpAttackCheckpoint;
    // [SerializeField] private float attckAgentStopTime;
    public int currentHp;
    [SerializeField] private int maxHp;
    // meshes
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRender;
    
    private bool isHunting;
    private float minAttackingInterval;
    private float attackingIntervalCounter;
    private Animator animator;
    private Vector3 jumpSpeed;
    private bool isJumping;
    private float jumpAttackCounter;
    
    // zombie had dead once, so it's second death
    private bool isDeadAgain;
    
    private bool isWithinAttackRange;
    
    // animator params to string
    private int animatorWalkingString;
    private int animatorDyingString;
    private int animatorAttackingString;
    

    private void Awake()
    {
        base.Awake();
        

        isWithinAttackRange = false;
        isHunting = false;
        isDeadAgain = false;
        minAttackingInterval = 1 / attackSpeed;
        attackingIntervalCounter = 0f;
        animator = GetComponent<Animator>();
        jumpSpeed = Vector3.zero;
        isJumping = false;
        jumpAttackCounter = 0;

        animatorWalkingString = Animator.StringToHash("WalkingSpeed");
        animatorDyingString = Animator.StringToHash("dying");
        animatorAttackingString = Animator.StringToHash("attacking");
        
    }

    void Start()
    {
        base.Start();
        skinnedMeshRender.sharedMesh = GameManager.Instance.aBloader.zombieMeshesObject.NromalZombieMeshes[
            Random.Range(0, GameManager.Instance.aBloader.zombieMeshesObject.NromalZombieMeshes.Length-1)];
        
        // test
        // JumpAttack(target.transform.position);
    }

    private void Update()
    {
        jumpAttackCounter -= Time.deltaTime;
        StopHuntIfPlayerOutOfHuntingRange();
        TryJumpAttackIfWithinRange();
        AttackPlayerIfWithingAttackingRange();
        CheckIfDead();
        
        // Debug.Log(agent.remainingDistance);
    }

    private void TryJumpAttackIfWithinRange()
    {
        if (!isHunting)
        {
            return;
        }
        if (jumpAttackCounter >= 0)
        {
            return;
        }

       
        // stopped by high obstacle
        float _distance = Vector3.Distance(transform.position, target.transform.position);
        Vector3 dir = (target.transform.position - jumpAttackCheckpoint.position).normalized;
        
        RaycastHit[] hits = Physics.RaycastAll(jumpAttackCheckpoint.position, dir, _distance);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == GameManager.Instance.highObstacleLayerMask)
            {
                // Debug.Log("stopped by high obstacle");
                return;
            }
        }
        
        
        
        
        if ( _distance<= jumpAttackRange && _distance >= attackRange)
            
        {
            JumpAttack(target.transform.position);
        }
    }


    private void OnAnimatorMove()
    {
        transform.position += animator.deltaPosition;
        agent.nextPosition = transform.position;
        // Debug.Log(agent.nextPosition);
    }

    // 超出范围则停止hunt玩家 or within attack range
    private void StopHuntIfPlayerOutOfHuntingRange()
    {
        if (isJumping)
        {
            return;
        }
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

    private void AttackPlayerIfWithingAttackingRange()
    {
        if (isJumping)
        {
            return;
        }
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

    private void Attack()
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
            // Debug.Log("jump zombie die");
        }
        
        
        animator.SetTrigger(animatorDyingString);
        if (agent.enabled)
        {
            agent.isStopped = true;
        }
        isDeadAgain = true;
        StopCoroutine(navCoro);
        agent.enabled = false;

        StartCoroutine(CoroDeadRemoveLastComponents());

    }

    public int AttackPower
    {
        get => attackPower;
        private set => this.attackPower = value;
    }

    public int Defense()
    {
        return defense;
    }

    
    // jump attack part
    private void JumpAttack(Vector3 destination)
    {
        jumpAttackCounter = jumpAttackCoolDown;
        jumpSpeed = (destination - transform.position) / 3 / 20;
        animator.SetTrigger("jumpAttack");
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            transform.position += jumpSpeed;
        }
    }


    public void JumpAttackStart()
    {
        // Debug.Log("jump attack!");
        isJumping = true;
        animator.applyRootMotion = false;
        agent.enabled = false;
        obstacle.enabled = false;
    }

    public void JumpAttackEnd()
    {
        // Debug.Log("jump attack end!");
        isJumping = false;
        animator.applyRootMotion = true;
        jumpSpeed = Vector3.zero;
        
        NavAgentChangeToObstacle();
    }

    int IZombieAttackPower.AttackPower()
    {
        return attackPower;
    }
    
    private IEnumerator CoroDeadRemoveLastComponents()
    {
        yield return new WaitForSeconds(4);
        Collider _collider = GetComponent<Collider>();
        Destroy(_collider);
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
        Destroy(animator);
        Destroy(agent);
        
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(jumpAttackCheckpoint.position, target.position);
    // }
}
}

