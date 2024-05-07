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
        attackPower = GetComponentInParent<IZombieAttackPower>().AttackPower();
        playerLayerMask = LayerMask.NameToLayer("Player") ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayerMask)
        {
            BattleProperties _other = other.GetComponent<PlayerMovement>() as BattleProperties;
            _other?.GetHit(attackPower);
        }
    }
    
    
}
}
