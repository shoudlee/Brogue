using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Brogue.Player;

namespace Brogue.Zombie{
public class ZombieAttackHands : MonoBehaviour
{
    public int attackPower;

    private int playerLayerMask;

    private void Start()
    {
        attackPower = GetComponentInParent<NormalZombie>().AttackPower;
        playerLayerMask = LayerMask.NameToLayer("Player") ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayerMask)
        {
            IBattleProperties _other = other.GetComponent<PlayerMovement>() as IBattleProperties;
            _other?.GetHit(attackPower);
        }
    }
    
    
}
}
