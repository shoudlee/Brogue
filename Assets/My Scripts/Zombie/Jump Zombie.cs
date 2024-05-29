using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Core;
using Brogue.Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace  Brogue.Zombie{
public class JumpZombie : NormalZombie
{
     
    [Header("Jumbie Attributes")]
    [SerializeField] private float jumpAttackCoolDown;
    [SerializeField] private int jumpAttackRange;
    [SerializeField] private Transform jumpAttackCheckpoint;
    
    private Vector3 jumpSpeed;
    private bool isJumping;
    private float jumpAttackCounter;
    

    protected override void Awake()
    {
        base.Awake();
        
        jumpSpeed = Vector3.zero;
        isJumping = false;
        jumpAttackCounter = 0;
        
    }

    protected override void Start()
    {
        base.Start();
        skinnedMeshRender.sharedMesh = GameManager.Instance.GetRandomZombieMesh("Jumbie");
    }

    protected override void Update()
    {
        base.Update();
        jumpAttackCounter -= Time.deltaTime;
        TryJumpAttackIfWithinRange();
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

    
    
    protected override void StopHuntIfPlayerOutOfHuntingRange()
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


    
    // called by animator
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
    

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(jumpAttackCheckpoint.position, target.position);
    // }
}
}

