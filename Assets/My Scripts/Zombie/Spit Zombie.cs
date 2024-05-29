using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Brogue.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Brogue.Player;
using Random = UnityEngine.Random;

namespace Brogue.Zombie
{
    

[RequireComponent(typeof(NavMeshAgent))]
public class SpitZombie : NormalZombie
{
    
    [Header("Spitbie Attributes")]
    [SerializeField] private int spitRange;
    [SerializeField] private int spitCoolDown;
    [SerializeField] private Transform spitCheckPoint;
    [SerializeField] private Transform spitableProjectile;
    private float spitCounter;
    private ProjectileSpitbie spit;
    
    // animator params to string
    private int animatorSpitAttack;

    protected override void Awake()
    {
        base.Awake();
        

        animator = GetComponent<Animator>();
        spitCounter = 0;

        animatorWalkingString = Animator.StringToHash("WalkingSpeed");
        animatorDyingString = Animator.StringToHash("dying");
        animatorAttackingString = Animator.StringToHash("attacking");
        animatorSpitAttack = Animator.StringToHash("spitAttack");

    }

    protected override void  Start()
    {
        base.Start();
        skinnedMeshRender.sharedMesh = GameManager.Instance.GetRandomZombieMesh("Spitbie");
    }

    protected override void Update()
    {
        base.Update();
        TrySpitIfWithingSpitRange();
        spitCounter -= Time.deltaTime;
    }

    private void TrySpitIfWithingSpitRange()
    {
        if (!isHunting)
        {
            return;
        }
        if (spitCounter >= 0)
        {
            return;
        }

       
        // stopped by high obstacle
        float _distance = Vector3.Distance(transform.position, target.transform.position);
        Vector3 dir = (target.transform.position - transform.position).normalized;
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, _distance);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == GameManager.Instance.highObstacleLayerMask)
            {
                // Debug.Log("stopped by high obstacle");
                return;
            }
        }

        if ( _distance<= spitRange && _distance >= attackRange)
            
        {
            SpitAttack();
            spitCounter = spitCoolDown;
        }
    }


    private void SpitAttack()
    {
        spit = Instantiate(spitableProjectile,spitCheckPoint).GetComponent<ProjectileSpitbie>();
        spit.transform.localPosition = Vector3.zero;
        spit.DisableGravity();
        animator.SetTrigger(animatorSpitAttack);
    }

    // called by animator
    public void AnimatorSpit()
    {
        if (spit != null)
        {
            spit.transform.parent = null;
            spit.LaunchProjectile(target.position);
            spit.EnableGravity();
            spit = null;
        }
    }
    
    
}
}