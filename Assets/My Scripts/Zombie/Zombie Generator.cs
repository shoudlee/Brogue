using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Player;
using Brogue.Zombie;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieGenerator : MonoBehaviour
{
    public int zombieCount;
    [SerializeField] private Transform target;
    [Space(10)]
    [Header("Zombies")]
    [Range(50, 300)]
    [SerializeField] private int maxZombies = 100;
    [SerializeField] private Transform normalZombie;
    [SerializeField] private Transform jumbie;
    [Space(10)] [SerializeField] private float radius;
    [Space(10)]
    
    // ratio of special zombie : normal zombie
    [Range(0.1f, 1)]
    [SerializeField] private float ratio = 0.5f;

    [SerializeField] private float genInterval = 2.5f;
    
    [SerializeField] private bool isActing;
    
    private float genCounter;
    
    private void Awake()
    {
        genCounter = 0;
        zombieCount = 0;
    }

    void Start()
    {
        target = GameManager.FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        if (isActing)
        {
            genCounter -= Time.deltaTime;
            if (genCounter <= 0)
            {
                genCounter = genInterval;
                
                if (zombieCount >= maxZombies)
                {
                    return;
                }
                
                Vector2 _pos2 = Random.insideUnitCircle.normalized;
                Vector3 _pos = new Vector3(_pos2.x, 0, _pos2.y);
                if (Random.Range(0.1f, 1) <= ratio)
                {
                    //gen a special zombie
                    Instantiate(jumbie, _pos*radius + target.position, Quaternion.identity).GetComponent<BaseEnemyClass>().Init(target, this);
                }
                else
                {
                    //gen a normal zombie
                    Instantiate(normalZombie, _pos*radius+ target.position, Quaternion.identity).GetComponent<BaseEnemyClass>().Init(target, this);
                }

                zombieCount++;
                
            }
        }
    }

}
