using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Core;
using UnityEngine;
using Brogue.Zombie;

namespace Brogue.Bullet{
public class NormalBullet : MonoBehaviour
{

    // have defaults
    [SerializeField] private float surviveTime = 3f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float beatBack = 1f;

    // components
    private TrailRenderer trailRenderer;


    // properties
    private Vector3 movingDistance;
    private int enemyLayer;
    private int enviromentLayer;
    private int highObstacleLayer;


    // call this when instantiate
    public void InitBullet(Vector3 direction)
    {
        movingDistance = direction.normalized * speed;
        StartCoroutine(DestroyWhenLifeTimeOut());
        trailRenderer.enabled = true;
    }



    private void Awake()
    {
        movingDistance = Vector3.zero;
        enemyLayer = LayerMask.NameToLayer("Enemy");
        enviromentLayer = LayerMask.NameToLayer("Enviroments");
        highObstacleLayer = LayerMask.NameToLayer("High Obstacle");
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    

    private void FixedUpdate()
    {
        transform.position += movingDistance;
    }
    
    private IEnumerator DestroyWhenLifeTimeOut()
    {
        yield return new WaitForSeconds(surviveTime);
        trailRenderer.enabled = false;
        BulletPool.Instance.ReturnNormalBullet(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        int _layer = other.gameObject.layer;

        if (_layer == enemyLayer)
        {
            IBattleProperties _zombie = other.gameObject.GetComponent<IBattleProperties>();

            
            DoDamage(_zombie);
            Debug.Log(other.gameObject.name);
            BeatBack(_zombie);
            // Destroy(gameObject);
            trailRenderer.enabled = false;
            BulletPool.Instance.ReturnNormalBullet(this);
            
        }else if (_layer == enviromentLayer || _layer == highObstacleLayer)
        {
            // Destroy(gameObject);
            trailRenderer.enabled = false;
            BulletPool.Instance.ReturnNormalBullet(this);
        }
    }
    
    
    
    private void DoDamage(IBattleProperties _zombie)
    {
        if (_zombie is not null)
        {
            _zombie.GetHit(damage);
        }
    }

    private void BeatBack(IBattleProperties zombie)
    {
        Vector3 beatBackDistance = movingDistance * beatBack;
        beatBackDistance.y = 0;
        if (zombie is not null)
        {
            zombie.Transform().position += beatBackDistance;
        }
    }
    
}
}