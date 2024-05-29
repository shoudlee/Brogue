using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Player;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

// y at -0.53 at the beginning, -0.1 at max spreading area
public class ProjectileSpitbieAffectedArea: MonoBehaviour
{
    [Header("Battle Parameters")]
    [SerializeField] private float duration;
    [SerializeField] private float spreadingSpeed;
    [SerializeField] private int damageLevel;
    [SerializeField] private float deceleration;

    private float maxY = -0.1f;

    private void Awake()
    {
        StartCoroutine(CoroDestroyAfterDuration());
    }
    
    
    private void FixedUpdate()
    {
        if (transform.position.y <= maxY)
        {
            float offset = spreadingSpeed * 1;
            Vector3 offsetedPosition = new Vector3(transform.position.x, transform.position.y + spreadingSpeed,
                transform.position.z);
            transform.position = offsetedPosition;
        }
        
    }

    private IEnumerator CoroDestroyAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        HandleDestroy();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameManager.Instance.playerLayerMask)
        { 
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            player.poisonList.Add(gameObject);
            player.PlayerInPoison(damageLevel);
            player.movementAlter = deceleration;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == GameManager.Instance.playerLayerMask)
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            player.poisonList.Remove(gameObject);
            player.PlayerOutOfPoison(damageLevel);
            player.movementAlter = 1;
        }
    }

    private void HandleDestroy()
    {
        if (GameManager.Instance.playerMovement.poisonList.Contains(gameObject))
        {
            GameManager.Instance.playerMovement.poisonList.Remove(gameObject);
            GameManager.Instance.playerMovement.PlayerOutOfPoison(damageLevel);
            GameManager.Instance.playerMovement.movementAlter = 1;
        }
    }
}
