using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileSpitbie : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private new SphereCollider collider;
    [SerializeField] private ProjectileSpitbieAffectedArea projectileSpitbieAffectedArea;
    
    // when this hit the ground, instantiate this area
    [SerializeField] private Transform affectedArea;
    
    
    // physical calculation parameters;
    // 重力加速度
    private float gravity = 9.81f;
    // xz速度 手动个控制
    [SerializeField] private float speedXZ = 2f;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameManager.Instance.mainFloorLayerMask
            | other.gameObject.layer == GameManager.Instance.playerLayerMask
            | other.gameObject.layer == GameManager.Instance.playerBulletLayerMask) 
        {
            // Debug.Log(Time.time);
            Instantiate(projectileSpitbieAffectedArea, new Vector3(transform.position.x, -0.53f, transform.position.z),
                Quaternion.identity);
            
            Destroy(gameObject);
        }else if (other.gameObject.layer == GameManager.Instance.highObstacleLayerMask)
        {
            Destroy(gameObject);
        }
    }

    // a spitbie throw this from his position to player's current position
    // spitbie instantiate a projector and call LaunchProjectile to player's current position just after
    // LaunchProjectile is used to calculate the velocity and set it to rigidbody
    // it is not accurate! so i have to add a Random.Range(0, 10) to modify
    public void LaunchProjectile(Vector3 targetPosition)
    {
        Vector3 projectileXZPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 targetXZPosition = new Vector3(targetPosition.x, 0f, targetPosition.z);
        float distance = Vector3.Distance(projectileXZPosition, targetXZPosition);
        float t = distance / speedXZ;
        // Debug.Log(t);
        
        float speedY = (-transform.position.y + gravity * t * t /2) / t;
        Vector3 velocity = new Vector3(0, speedY + Random.Range(0, 10), speedXZ);
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
        velocity = transform.TransformVector(velocity);
        rb.velocity = velocity;
    }

    public void EnableGravity()
    {
        rb.useGravity = true;
    }

    public void DisableGravity()
    {
        rb.useGravity = false;
    }
}
