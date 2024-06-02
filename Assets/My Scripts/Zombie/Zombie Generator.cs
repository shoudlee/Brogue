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
    private Transform target;
    [Space(10)]
    
    [Header("Zombies")]
    [Range(50, 300)]
    
    [SerializeField] private int maxZombies = 500;
    [SerializeField] private Transform normalZombie;
    [SerializeField] private Transform jumbie;
    [SerializeField] private Transform Spitbie;
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
        target = FindObjectOfType<PlayerMovement>().transform;
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
                    // gen a special zombie
                     if (Random.Range(0.1f, 1) <= 0.5f)
                     {
                         Instantiate(jumbie, _pos*radius + target.position, Quaternion.identity).GetComponent<BaseEnemyClass>().Init(target, this);
                     }
                     else
                     {
                         Instantiate(Spitbie, _pos * radius + target.position, Quaternion.identity)
                             .GetComponent<BaseEnemyClass>().Init(target, this);
                     }
                    // Instantiate(jumbie, _pos*radius + target.position, Quaternion.identity).GetComponent<BaseEnemyClass>().Init(target, this);

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
