using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpitbie : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider collider;
    
    // when this hit the ground, instantiate this area
    [SerializeField] private Transform affectedArea;
    
    
    // physical calculation parameters;
    // 重力加速度
    [SerializeField] private float gravity = 9.8f;
    // 发射角度
    [SerializeField] private float firingAngle = 45.0f;
    // xz速度 手动个控制
    [SerializeField] private float speedXZ = 2f;

    private void Start()
    {
        LaunchProjectile(new Vector3(-11,0.3f, 28));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameManager.Instance.mainFloorLayerMask
            | other.gameObject.layer == GameManager.Instance.playerLayerMask
            | other.gameObject.layer == GameManager.Instance.playerBulletLayerMask) 
        {
            Destroy(gameObject);
            Debug.Log("instantiate the area!");
        }
    }

    // a spitbie throw this from his position to player's current position
    // spitbie instantiate a projector and call LaunchProjectile to player's current position just after
    // LaunchProjectile is used to calculate the velocity and set it to rigidbody
    private void LaunchProjectile(Vector3 targetPosition)
    {
        Vector3 projectileXZPosition = transform.position;
        Vector3 targetXZPosition = new Vector3(targetPosition.x, 0f, targetPosition.z);
        float distance = Vector3.Distance(projectileXZPosition, targetXZPosition);
        float t = distance / speedXZ;

        float speedY = (float)(transform.position.y - collider.radius + 0.5f * gravity * Math.Pow(t, 2)) / t;
        Vector3 velocity = new Vector3(0, speedY, speedXZ);
        transform.LookAt(new Vector3(targetXZPosition.x, transform.position.y, targetPosition.z));
        velocity = transform.TransformVector(velocity);
        rb.velocity = velocity;
    }
}
