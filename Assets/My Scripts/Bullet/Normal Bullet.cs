using System;
using System.Collections;
using System.Collections.Generic;
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
    private Collider col;
    private TrailRenderer trailRenderer;


    // properties
    private Vector3 movingDistance;
    private int enemyLayer;
    private int enviromentLayer;


    // call this when instantiate
    public void InitBullet(Vector3 direction)
    {
        movingDistance = direction.normalized * speed;
        StartCoroutine(DestroyWhenLifeTimeOut());
        trailRenderer.enabled = true;
    }



    private void Awake()
    {
        if (!TryGetComponent<Collider>(out col))
        {
            Debug.Log("Not all components set up");
        }

        movingDistance = Vector3.zero;
        enemyLayer = LayerMask.NameToLayer("Enemy");
        enviromentLayer = LayerMask.NameToLayer("Enviroments");
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    

    private void FixedUpdate()
    {
        transform.position += movingDistance;
    }
    
    private IEnumerator DestroyWhenLifeTimeOut()
    {
        yield return new WaitForSeconds(surviveTime);
        BulletPool.Instance.ReturnNormalBullet(this);
        trailRenderer.enabled = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        int _layer = other.gameObject.layer;

        if (_layer == enemyLayer)
        {
            IZombieHitable _zombie = other.gameObject.GetComponent<IZombieHitable>();

            
            DoDamage(_zombie);
            BeatBack(_zombie);
            // Destroy(gameObject);
            BulletPool.Instance.ReturnNormalBullet(this);
            trailRenderer.enabled = false;
            return;
            
        }

        if (_layer == enviromentLayer)
        {
            // Destroy(gameObject);
            BulletPool.Instance.ReturnNormalBullet(this);
            trailRenderer.enabled = false;
        }
    }
    
    
    
    private void DoDamage(IZombieHitable _zombie)
    {
        if (_zombie is not null)
        {
            _zombie.GetHit(damage);
        }
    }

    private void BeatBack(IZombieHitable zombie)
    {
        Vector3 beatBackDistance = movingDistance * beatBack;
        beatBackDistance.y = 0;
        if (zombie is not null)
        {
            zombie.GetPosition().position += beatBackDistance;
        }
    }
    
}
}